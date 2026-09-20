import { pool } from "../db/pool.js";
import { ApiError } from "../utils/apiError.js";
import { BERLET_KATEGORIA, BERLET_STATUSZ } from "../utils/enums.js";
import { toNumberOrNull } from "../utils/szam.js";

const LEJARO_KUSZOB_NAPOKBAN = 7;

function toTipusResponse(row) {
  return {
    id: row.Id,
    nev: row.Nev,
    kategoria: BERLET_KATEGORIA.fromDb(row.Kategoria),
    ar: toNumberOrNull(row.Ar),
    alapertelmezettAlkalmak: row.AlapertelmezettAlkalmak,
    aktiv: !!row.Aktiv,
  };
}

function toBerletResponse(row) {
  return {
    id: row.Id,
    tagId: row.TagId,
    tagNev: `${row.TagVezeteknev} ${row.TagKeresztnev}`,
    berletTipusNev: row.BerletTipusNev,
    kezdoDatum: row.KezdoDatum,
    lejaratiDatum: row.LejaratiDatum,
    arFizetve: toNumberOrNull(row.ArFizetve),
    hatralevoAlkalmak: row.HatralevoAlkalmak,
    statusz: BERLET_STATUSZ.fromDb(row.Statusz),
    lejaro: row.Lejaro === 1,
  };
}

const BERLET_SELECT = `
  SELECT b.*, t.Vezeteknev AS TagVezeteknev, t.Keresztnev AS TagKeresztnev, bt.Nev AS BerletTipusNev,
         (b.LejaratiDatum <= DATE_ADD(CURDATE(), INTERVAL ${LEJARO_KUSZOB_NAPOKBAN} DAY)) AS Lejaro
  FROM berletek b
  JOIN tagok t ON t.Id = b.TagId
  JOIN berlettipusok bt ON bt.Id = b.BerletTipusId`;

export async function getTipusok(req, res, next) {
  try {
    const [rows] = await pool.query("SELECT * FROM berlettipusok");
    res.json(rows.map(toTipusResponse));
  } catch (err) {
    next(err);
  }
}

export async function createTipus(req, res, next) {
  try {
    const b = req.body;
    const [result] = await pool.query(
      "INSERT INTO berlettipusok (Nev, Kategoria, Ar, AlapertelmezettAlkalmak, Aktiv) VALUES (?, ?, ?, ?, ?)",
      [b.nev, BERLET_KATEGORIA.toDb(b.kategoria), b.ar, b.alapertelmezettAlkalmak ?? null, b.aktiv ? 1 : 0]
    );
    const [rows] = await pool.query("SELECT * FROM berlettipusok WHERE Id=?", [result.insertId]);
    res.status(201).json(toTipusResponse(rows[0]));
  } catch (err) {
    next(err);
  }
}

export async function updateTipus(req, res, next) {
  try {
    const [existing] = await pool.query("SELECT Id FROM berlettipusok WHERE Id=?", [req.params.id]);
    if (!existing[0]) throw ApiError.notFound("Bérlettípus nem található.");

    const b = req.body;
    await pool.query(
      "UPDATE berlettipusok SET Nev=?, Kategoria=?, Ar=?, AlapertelmezettAlkalmak=?, Aktiv=? WHERE Id=?",
      [b.nev, BERLET_KATEGORIA.toDb(b.kategoria), b.ar, b.alapertelmezettAlkalmak ?? null, b.aktiv ? 1 : 0, req.params.id]
    );

    const [rows] = await pool.query("SELECT * FROM berlettipusok WHERE Id=?", [req.params.id]);
    res.json(toTipusResponse(rows[0]));
  } catch (err) {
    next(err);
  }
}

export async function hozzarendeles(req, res, next) {
  try {
    const b = req.body;

    const [tag] = await pool.query("SELECT Id FROM tagok WHERE Id=? AND Aktiv=1", [b.tagId]);
    if (!tag[0]) throw ApiError.notFound("Tag nem található, vagy inaktív.");

    const [tipusRows] = await pool.query("SELECT * FROM berlettipusok WHERE Id=?", [b.berletTipusId]);
    const tipus = tipusRows[0];
    if (!tipus) throw ApiError.notFound("Bérlettípus nem található.");

    const [result] = await pool.query(
      `INSERT INTO berletek (TagId, BerletTipusId, KezdoDatum, LejaratiDatum, ArFizetve, HatralevoAlkalmak, Statusz, LetrehozvaAltalId)
       VALUES (?, ?, ?, ?, ?, ?, ?, ?)`,
      [
        b.tagId,
        b.berletTipusId,
        b.kezdoDatum,
        b.lejaratiDatum,
        b.arFelulir ?? tipus.Ar,
        b.alkalmakFelulir ?? tipus.AlapertelmezettAlkalmak,
        BERLET_STATUSZ.toDb("Aktiv"),
        req.user.sub,
      ]
    );

    const [rows] = await pool.query(`${BERLET_SELECT} WHERE b.Id=?`, [result.insertId]);
    res.json(toBerletResponse(rows[0]));
  } catch (err) {
    next(err);
  }
}

export async function getBerletekByTagId(tagId) {
  const [rows] = await pool.query(`${BERLET_SELECT} WHERE b.TagId=? ORDER BY b.KezdoDatum DESC`, [tagId]);
  return rows.map(toBerletResponse);
}

export async function getTagBerletei(req, res, next) {
  try {
    res.json(await getBerletekByTagId(req.params.tagId));
  } catch (err) {
    next(err);
  }
}

export async function getSajatBerlet(req, res, next) {
  try {
    res.json(await getBerletekByTagId(req.user.tagId));
  } catch (err) {
    next(err);
  }
}

export async function getLejarok(req, res, next) {
  try {
    const [rows] = await pool.query(
      `${BERLET_SELECT} WHERE b.Statusz=0 AND b.LejaratiDatum <= DATE_ADD(CURDATE(), INTERVAL ${LEJARO_KUSZOB_NAPOKBAN} DAY) ORDER BY b.LejaratiDatum`
    );
    res.json(rows.map(toBerletResponse));
  } catch (err) {
    next(err);
  }
}
