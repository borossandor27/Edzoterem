import { pool } from "../db/pool.js";
import { ApiError } from "../utils/apiError.js";
import { NEM, HET_NAPJA } from "../utils/enums.js";
import { toNumberOrNull } from "../utils/szam.js";

function toDolgozoResponse(row) {
  return {
    id: row.Id,
    vezeteknev: row.Vezeteknev,
    keresztnev: row.Keresztnev,
    becenev: row.Becenev,
    munkakor: row.MunkakorNev,
    telefon: row.Telefon,
    email: row.Email,
    szuletesiDatum: row.SzuletesiDatum,
    nem: NEM.fromDb(row.Nem),
    oradij: toNumberOrNull(row.Oradij),
    aktiv: !!row.Aktiv,
  };
}

const DOLGOZO_SELECT = `SELECT d.*, m.Nev AS MunkakorNev FROM dolgozok d JOIN munkakorok m ON m.Id = d.MunkakorId`;

export async function getAll(req, res, next) {
  try {
    const [rows] = await pool.query(`${DOLGOZO_SELECT} ORDER BY d.Vezeteknev, d.Keresztnev`);
    res.json(rows.map(toDolgozoResponse));
  } catch (err) {
    next(err);
  }
}

export async function getById(req, res, next) {
  try {
    const [rows] = await pool.query(`${DOLGOZO_SELECT} WHERE d.Id = ?`, [req.params.id]);
    if (!rows[0]) throw ApiError.notFound("Dolgozó nem található.");
    res.json(toDolgozoResponse(rows[0]));
  } catch (err) {
    next(err);
  }
}

export async function create(req, res, next) {
  try {
    const b = req.body;
    const [result] = await pool.query(
      `INSERT INTO dolgozok
        (Vezeteknev, Keresztnev, Becenev, MunkakorId, SzemelyiSzam, TajSzam, Adoszam, Telefon, Email,
         SzuletesiDatum, Nem, FenykepUrl, PortfolioUrl, Oradij, Megjegyzes, Aktiv, Letrehozva)
       VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, 1, NOW())`,
      [
        b.vezeteknev,
        b.keresztnev,
        b.becenev ?? null,
        b.munkakorId,
        b.szemelyiSzam ?? null,
        b.tajSzam ?? null,
        b.adoszam ?? null,
        b.telefon ?? null,
        b.email ?? null,
        b.szuletesiDatum,
        NEM.toDb(b.nem),
        b.fenykepUrl ?? null,
        b.portfolioUrl ?? null,
        b.oradij ?? null,
        b.megjegyzes ?? null,
      ]
    );

    const [rows] = await pool.query(`${DOLGOZO_SELECT} WHERE d.Id = ?`, [result.insertId]);
    res.status(201).json(toDolgozoResponse(rows[0]));
  } catch (err) {
    next(err);
  }
}

export async function update(req, res, next) {
  try {
    const b = req.body;
    const [existing] = await pool.query("SELECT Id FROM dolgozok WHERE Id = ?", [req.params.id]);
    if (!existing[0]) throw ApiError.notFound("Dolgozó nem található.");

    await pool.query(
      `UPDATE dolgozok SET
        Vezeteknev=?, Keresztnev=?, Becenev=?, MunkakorId=?, SzemelyiSzam=?, TajSzam=?, Adoszam=?,
        Telefon=?, Email=?, SzuletesiDatum=?, Nem=?, FenykepUrl=?, PortfolioUrl=?, Oradij=?, Megjegyzes=?,
        Modositva=NOW()
       WHERE Id=?`,
      [
        b.vezeteknev,
        b.keresztnev,
        b.becenev ?? null,
        b.munkakorId,
        b.szemelyiSzam ?? null,
        b.tajSzam ?? null,
        b.adoszam ?? null,
        b.telefon ?? null,
        b.email ?? null,
        b.szuletesiDatum,
        NEM.toDb(b.nem),
        b.fenykepUrl ?? null,
        b.portfolioUrl ?? null,
        b.oradij ?? null,
        b.megjegyzes ?? null,
        req.params.id,
      ]
    );

    const [rows] = await pool.query(`${DOLGOZO_SELECT} WHERE d.Id = ?`, [req.params.id]);
    res.json(toDolgozoResponse(rows[0]));
  } catch (err) {
    next(err);
  }
}

export async function kileptetes(req, res, next) {
  try {
    const [result] = await pool.query("UPDATE dolgozok SET Aktiv=0, Modositva=NOW() WHERE Id=?", [
      req.params.id,
    ]);
    if (result.affectedRows === 0) throw ApiError.notFound("Dolgozó nem található.");
    res.status(204).send();
  } catch (err) {
    next(err);
  }
}

export async function getMunkarend(req, res, next) {
  try {
    const [rows] = await pool.query(
      "SELECT * FROM munkarendek WHERE DolgozoId=? ORDER BY Datum",
      [req.params.id]
    );
    res.json(
      rows.map((r) => ({
        id: r.Id,
        datum: r.Datum,
        tolIdo: r.TolIdo,
        igIdo: r.IgIdo,
        megjegyzes: r.Megjegyzes,
      }))
    );
  } catch (err) {
    next(err);
  }
}

export async function addMunkarend(req, res, next) {
  try {
    const dolgozoId = req.params.id;
    const [dolgozo] = await pool.query("SELECT Id FROM dolgozok WHERE Id=?", [dolgozoId]);
    if (!dolgozo[0]) throw ApiError.notFound("Dolgozó nem található.");

    const b = req.body;
    const [result] = await pool.query(
      "INSERT INTO munkarendek (DolgozoId, Datum, TolIdo, IgIdo, Megjegyzes) VALUES (?, ?, ?, ?, ?)",
      [dolgozoId, b.datum, b.tolIdo, b.igIdo, b.megjegyzes ?? null]
    );

    res.status(201).json({ id: result.insertId, datum: b.datum, tolIdo: b.tolIdo, igIdo: b.igIdo, megjegyzes: b.megjegyzes ?? null });
  } catch (err) {
    next(err);
  }
}

// --- Edző al-erőforrás ---

function toEdzoResponse(row) {
  return {
    id: row.Id,
    dolgozoId: row.DolgozoId,
    nev: `${row.Vezeteknev} ${row.Keresztnev}`,
    szakterulet: row.Szakterulet,
    vegzettseg: row.Vegzettseg,
    oradij: toNumberOrNull(row.Oradij),
  };
}

const EDZO_SELECT = `SELECT e.*, d.Vezeteknev, d.Keresztnev, d.Aktiv AS DolgozoAktiv FROM edzok e JOIN dolgozok d ON d.Id = e.DolgozoId`;

export async function getEdzok(req, res, next) {
  try {
    const [rows] = await pool.query(`${EDZO_SELECT} WHERE d.Aktiv = 1`);
    res.json(rows.map(toEdzoResponse));
  } catch (err) {
    next(err);
  }
}

export async function createEdzo(req, res, next) {
  try {
    const dolgozoId = req.params.dolgozoId;
    const [dolgozo] = await pool.query("SELECT Id FROM dolgozok WHERE Id=?", [dolgozoId]);
    if (!dolgozo[0]) throw ApiError.notFound("Dolgozó nem található.");

    const [meglevo] = await pool.query("SELECT Id FROM edzok WHERE DolgozoId=?", [dolgozoId]);
    if (meglevo[0]) throw ApiError.conflict("Ehhez a dolgozóhoz már tartozik edző rekord.");

    const b = req.body;
    const [result] = await pool.query(
      "INSERT INTO edzok (DolgozoId, Szakterulet, Vegzettseg, Oradij) VALUES (?, ?, ?, ?)",
      [dolgozoId, b.szakterulet ?? null, b.vegzettseg ?? null, b.oradij ?? null]
    );

    const [rows] = await pool.query(`${EDZO_SELECT} WHERE e.Id=?`, [result.insertId]);
    res.status(201).json(toEdzoResponse(rows[0]));
  } catch (err) {
    next(err);
  }
}

export async function updateEdzo(req, res, next) {
  try {
    const edzoId = req.params.edzoId;
    const [existing] = await pool.query("SELECT Id FROM edzok WHERE Id=?", [edzoId]);
    if (!existing[0]) throw ApiError.notFound("Edző nem található.");

    const b = req.body;
    await pool.query("UPDATE edzok SET Szakterulet=?, Vegzettseg=?, Oradij=? WHERE Id=?", [
      b.szakterulet ?? null,
      b.vegzettseg ?? null,
      b.oradij ?? null,
      edzoId,
    ]);

    const [rows] = await pool.query(`${EDZO_SELECT} WHERE e.Id=?`, [edzoId]);
    res.json(toEdzoResponse(rows[0]));
  } catch (err) {
    next(err);
  }
}

export async function getElerhetoseg(req, res, next) {
  try {
    const [rows] = await pool.query("SELECT * FROM edzoelerhetosegek WHERE EdzoId=?", [
      req.params.edzoId,
    ]);
    res.json(
      rows.map((r) => ({
        id: r.Id,
        hetNapja: HET_NAPJA.fromDb(r.HetNapja),
        tolIdo: r.TolIdo,
        igIdo: r.IgIdo,
      }))
    );
  } catch (err) {
    next(err);
  }
}

export async function addElerhetoseg(req, res, next) {
  try {
    const edzoId = req.params.edzoId;

    if (req.user.szerepkor === "Edzo") {
      const sajatEdzoId = await getEdzoIdByDolgozoId(req.user.dolgozoId);
      if (sajatEdzoId !== Number(edzoId)) {
        throw ApiError.forbidden("Csak a saját elérhetőségedet módosíthatod.");
      }
    }

    const [edzo] = await pool.query("SELECT Id FROM edzok WHERE Id=?", [edzoId]);
    if (!edzo[0]) throw ApiError.notFound("Edző nem található.");

    const b = req.body;
    const hetNapjaDb = HET_NAPJA.toDb(b.hetNapja);
    const [result] = await pool.query(
      "INSERT INTO edzoelerhetosegek (EdzoId, HetNapja, TolIdo, IgIdo) VALUES (?, ?, ?, ?)",
      [edzoId, hetNapjaDb, b.tolIdo, b.igIdo]
    );

    res.status(201).json({ id: result.insertId, hetNapja: b.hetNapja, tolIdo: b.tolIdo, igIdo: b.igIdo });
  } catch (err) {
    next(err);
  }
}

export async function getEdzoIdByDolgozoId(dolgozoId) {
  const [rows] = await pool.query("SELECT Id FROM edzok WHERE DolgozoId=?", [dolgozoId]);
  if (!rows[0]) throw ApiError.notFound("A bejelentkezett felhasználóhoz nem tartozik edző rekord.");
  return rows[0].Id;
}
