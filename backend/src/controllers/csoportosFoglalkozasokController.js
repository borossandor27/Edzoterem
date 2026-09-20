import { pool } from "../db/pool.js";
import { ApiError } from "../utils/apiError.js";
import { CSOPORTOS_STATUSZ } from "../utils/enums.js";
import { toIso } from "../utils/datum.js";
import { getEdzoIdByDolgozoId } from "./dolgozokController.js";

const FOGLALKOZAS_SELECT = `
  SELECT c.*, d.Vezeteknev AS EdzoVezeteknev, d.Keresztnev AS EdzoKeresztnev
  FROM csoportosfoglalkozasok c
  JOIN edzok e ON e.Id = c.EdzoId
  JOIN dolgozok d ON d.Id = e.DolgozoId`;

async function getJelentkezesek(foglalkozasId) {
  const [rows] = await pool.query(
    `SELECT j.*, t.Vezeteknev, t.Keresztnev
     FROM csoportosjelentkezesek j
     JOIN tagok t ON t.Id = j.TagId
     WHERE j.CsoportosFoglalkozasId = ?`,
    [foglalkozasId]
  );
  return rows.map((r) => ({
    id: r.Id,
    tagId: r.TagId,
    tagNev: `${r.Vezeteknev} ${r.Keresztnev}`,
    jelenVolt: r.JelenVolt === null ? null : !!r.JelenVolt,
  }));
}

async function toResponse(row) {
  return {
    id: row.Id,
    megnevezes: row.Megnevezes,
    edzoId: row.EdzoId,
    edzoNev: `${row.EdzoVezeteknev} ${row.EdzoKeresztnev}`,
    idopont: toIso(row.Idopont),
    helyszin: row.Helyszin,
    maxLetszam: row.MaxLetszam,
    statusz: CSOPORTOS_STATUSZ.fromDb(row.Statusz),
    elutasitasIndoka: row.ElutasitasIndoka,
    jelentkezesek: await getJelentkezesek(row.Id),
  };
}

async function getByIdOrThrow(id) {
  const [rows] = await pool.query(`${FOGLALKOZAS_SELECT} WHERE c.Id=?`, [id]);
  if (!rows[0]) throw ApiError.notFound("Csoportos foglalkozás nem található.");
  return toResponse(rows[0]);
}

export async function getAll(req, res, next) {
  try {
    const [rows] = await pool.query(`${FOGLALKOZAS_SELECT} ORDER BY c.Idopont DESC`);
    res.json(await Promise.all(rows.map(toResponse)));
  } catch (err) {
    next(err);
  }
}

export async function getById(req, res, next) {
  try {
    res.json(await getByIdOrThrow(req.params.id));
  } catch (err) {
    next(err);
  }
}

export async function getEdzoSajatjai(edzoId) {
  const [rows] = await pool.query(`${FOGLALKOZAS_SELECT} WHERE c.EdzoId=? ORDER BY c.Idopont DESC`, [edzoId]);
  return Promise.all(rows.map(toResponse));
}

export async function meEdzoSajatjai(req, res, next) {
  try {
    const edzoId = await getEdzoIdByDolgozoId(req.user.dolgozoId);
    res.json(await getEdzoSajatjai(edzoId));
  } catch (err) {
    next(err);
  }
}

export async function meTagSajatjai(req, res, next) {
  try {
    const [rows] = await pool.query(
      `${FOGLALKOZAS_SELECT} WHERE c.Id IN (SELECT CsoportosFoglalkozasId FROM csoportosjelentkezesek WHERE TagId=?) ORDER BY c.Idopont DESC`,
      [req.user.tagId]
    );
    res.json(await Promise.all(rows.map(toResponse)));
  } catch (err) {
    next(err);
  }
}

export async function meghirdetes(req, res, next) {
  try {
    const edzoId = await getEdzoIdByDolgozoId(req.user.dolgozoId);

    const b = req.body;
    const [result] = await pool.query(
      `INSERT INTO csoportosfoglalkozasok
        (Megnevezes, EdzoId, Idopont, Helyszin, MaxLetszam, Statusz, MeghirdetteId)
       VALUES (?, ?, ?, ?, ?, ?, ?)`,
      [b.megnevezes, edzoId, new Date(b.idopont), b.helyszin, b.maxLetszam, CSOPORTOS_STATUSZ.toDb("JovahagyasraVar"), req.user.sub]
    );

    res.status(201).json(await getByIdOrThrow(result.insertId));
  } catch (err) {
    next(err);
  }
}

export async function jovahagyas(req, res, next) {
  try {
    const [rows] = await pool.query("SELECT Statusz FROM csoportosfoglalkozasok WHERE Id=?", [req.params.id]);
    if (!rows[0]) throw ApiError.notFound("Csoportos foglalkozás nem található.");
    if (rows[0].Statusz !== CSOPORTOS_STATUSZ.toDb("JovahagyasraVar")) {
      throw ApiError.conflict("Csak jóváhagyásra váró foglalkozás hagyható jóvá.");
    }

    await pool.query(
      "UPDATE csoportosfoglalkozasok SET Statusz=?, JovahagytaId=?, JovahagyasDatuma=NOW() WHERE Id=?",
      [CSOPORTOS_STATUSZ.toDb("Jovahagyva"), req.user.sub, req.params.id]
    );

    res.json(await getByIdOrThrow(req.params.id));
  } catch (err) {
    next(err);
  }
}

export async function elutasitas(req, res, next) {
  try {
    const [rows] = await pool.query("SELECT Statusz FROM csoportosfoglalkozasok WHERE Id=?", [req.params.id]);
    if (!rows[0]) throw ApiError.notFound("Csoportos foglalkozás nem található.");
    if (rows[0].Statusz !== CSOPORTOS_STATUSZ.toDb("JovahagyasraVar")) {
      throw ApiError.conflict("Csak jóváhagyásra váró foglalkozás utasítható el.");
    }

    await pool.query(
      "UPDATE csoportosfoglalkozasok SET Statusz=?, JovahagytaId=?, JovahagyasDatuma=NOW(), ElutasitasIndoka=? WHERE Id=?",
      [CSOPORTOS_STATUSZ.toDb("Elutasitva"), req.user.sub, req.body.indok ?? null, req.params.id]
    );

    res.json(await getByIdOrThrow(req.params.id));
  } catch (err) {
    next(err);
  }
}

export async function jelentkezes(req, res, next) {
  try {
    const foglalkozasId = req.params.id;
    const [foglalkozasRows] = await pool.query("SELECT * FROM csoportosfoglalkozasok WHERE Id=?", [foglalkozasId]);
    const foglalkozas = foglalkozasRows[0];
    if (!foglalkozas) throw ApiError.notFound("Csoportos foglalkozás nem található.");
    if (foglalkozas.Statusz !== CSOPORTOS_STATUSZ.toDb("Jovahagyva")) {
      throw ApiError.conflict("Csak jóváhagyott foglalkozásra lehet jelentkeztetni tagot.");
    }

    const tagId = req.body.tagId;
    const [tag] = await pool.query("SELECT Id FROM tagok WHERE Id=? AND Aktiv=1", [tagId]);
    if (!tag[0]) throw ApiError.notFound("Tag nem található, vagy inaktív.");

    const [meglevo] = await pool.query(
      "SELECT Id FROM csoportosjelentkezesek WHERE CsoportosFoglalkozasId=? AND TagId=?",
      [foglalkozasId, tagId]
    );
    if (meglevo[0]) throw ApiError.conflict("A tag már jelentkezett erre a foglalkozásra.");

    const [countRows] = await pool.query(
      "SELECT COUNT(*) AS db FROM csoportosjelentkezesek WHERE CsoportosFoglalkozasId=?",
      [foglalkozasId]
    );
    if (countRows[0].db >= foglalkozas.MaxLetszam) {
      throw ApiError.conflict("A foglalkozás betelt (elérte a maximális létszámot).");
    }

    await pool.query(
      "INSERT INTO csoportosjelentkezesek (CsoportosFoglalkozasId, TagId, JelentkezesDatuma, RogzitveAltalId) VALUES (?, ?, NOW(), ?)",
      [foglalkozasId, tagId, req.user.sub]
    );

    res.json(await getByIdOrThrow(foglalkozasId));
  } catch (err) {
    next(err);
  }
}

export async function jelentkezesTorlese(req, res, next) {
  try {
    const [result] = await pool.query(
      "DELETE FROM csoportosjelentkezesek WHERE CsoportosFoglalkozasId=? AND TagId=?",
      [req.params.id, req.params.tagId]
    );
    if (result.affectedRows === 0) throw ApiError.notFound("Jelentkezés nem található.");
    res.status(204).send();
  } catch (err) {
    next(err);
  }
}

export async function jelenlet(req, res, next) {
  try {
    const edzoId = await getEdzoIdByDolgozoId(req.user.dolgozoId);

    const [rows] = await pool.query("SELECT * FROM csoportosfoglalkozasok WHERE Id=?", [req.params.id]);
    const foglalkozas = rows[0];
    if (!foglalkozas) throw ApiError.notFound("Csoportos foglalkozás nem található.");
    if (foglalkozas.EdzoId !== edzoId) throw ApiError.forbidden("Csak a foglalkozást tartó edző rögzítheti a jelenlétet.");
    if (foglalkozas.Statusz !== CSOPORTOS_STATUSZ.toDb("Jovahagyva")) {
      throw ApiError.conflict("Csak jóváhagyott foglalkozáson rögzíthető jelenlét.");
    }

    for (const bejegyzes of req.body.jelenletek ?? []) {
      await pool.query(
        "UPDATE csoportosjelentkezesek SET JelenVolt=? WHERE CsoportosFoglalkozasId=? AND TagId=?",
        [bejegyzes.jelenVolt ? 1 : 0, req.params.id, bejegyzes.tagId]
      );
    }

    res.json(await getByIdOrThrow(req.params.id));
  } catch (err) {
    next(err);
  }
}

export async function lezaras(req, res, next) {
  try {
    const edzoId = await getEdzoIdByDolgozoId(req.user.dolgozoId);

    const [rows] = await pool.query("SELECT * FROM csoportosfoglalkozasok WHERE Id=?", [req.params.id]);
    const foglalkozas = rows[0];
    if (!foglalkozas) throw ApiError.notFound("Csoportos foglalkozás nem található.");
    if (foglalkozas.EdzoId !== edzoId) throw ApiError.forbidden("Csak a foglalkozást tartó edző zárhatja le.");
    if (foglalkozas.Statusz !== CSOPORTOS_STATUSZ.toDb("Jovahagyva")) {
      throw ApiError.conflict("Csak jóváhagyott foglalkozás zárható le.");
    }

    await pool.query("UPDATE csoportosfoglalkozasok SET Statusz=?, LezarasDatuma=NOW() WHERE Id=?", [
      CSOPORTOS_STATUSZ.toDb("Lezarva"),
      req.params.id,
    ]);

    res.json(await getByIdOrThrow(req.params.id));
  } catch (err) {
    next(err);
  }
}
