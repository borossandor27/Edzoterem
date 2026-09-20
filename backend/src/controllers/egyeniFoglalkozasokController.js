import { pool } from "../db/pool.js";
import { ApiError } from "../utils/apiError.js";
import { EGYENI_ALLAPOT } from "../utils/enums.js";
import { toIso } from "../utils/datum.js";
import { toNumberOrNull } from "../utils/szam.js";

function toTipusResponse(row) {
  return { id: row.Id, nev: row.Nev, alapertelmezettAr: toNumberOrNull(row.AlapertelmezettAr), aktiv: !!row.Aktiv };
}

const FOGLALKOZAS_SELECT = `
  SELECT f.*, s.Nev AS SzolgaltatasNev,
         d.Vezeteknev AS DolgozoVezeteknev, d.Keresztnev AS DolgozoKeresztnev,
         t.Vezeteknev AS TagVezeteknev, t.Keresztnev AS TagKeresztnev
  FROM egyenifoglalkozasok f
  JOIN szolgaltatastipusok s ON s.Id = f.SzolgaltatasTipusId
  JOIN dolgozok d ON d.Id = f.DolgozoId
  JOIN tagok t ON t.Id = f.TagId`;

function toResponse(row) {
  return {
    id: row.Id,
    szolgaltatasNev: row.SzolgaltatasNev,
    dolgozoId: row.DolgozoId,
    dolgozoNev: `${row.DolgozoVezeteknev} ${row.DolgozoKeresztnev}`,
    tagId: row.TagId,
    tagNev: `${row.TagVezeteknev} ${row.TagKeresztnev}`,
    idopont: toIso(row.Idopont),
    ar: toNumberOrNull(row.Ar),
    allapot: EGYENI_ALLAPOT.fromDb(row.Allapot),
    megjegyzes: row.Megjegyzes,
  };
}

export async function getSzolgaltatasTipusok(req, res, next) {
  try {
    const [rows] = await pool.query("SELECT * FROM szolgaltatastipusok");
    res.json(rows.map(toTipusResponse));
  } catch (err) {
    next(err);
  }
}

export async function createSzolgaltatasTipus(req, res, next) {
  try {
    const b = req.body;
    const [result] = await pool.query(
      "INSERT INTO szolgaltatastipusok (Nev, AlapertelmezettAr, Aktiv) VALUES (?, ?, ?)",
      [b.nev, b.alapertelmezettAr, b.aktiv ? 1 : 0]
    );
    const [rows] = await pool.query("SELECT * FROM szolgaltatastipusok WHERE Id=?", [result.insertId]);
    res.status(201).json(toTipusResponse(rows[0]));
  } catch (err) {
    next(err);
  }
}

export async function rogzites(req, res, next) {
  try {
    const b = req.body;

    const [tipusRows] = await pool.query("SELECT * FROM szolgaltatastipusok WHERE Id=?", [b.szolgaltatasTipusId]);
    const tipus = tipusRows[0];
    if (!tipus) throw ApiError.notFound("Szolgáltatástípus nem található.");

    const [dolgozo] = await pool.query("SELECT Id FROM dolgozok WHERE Id=? AND Aktiv=1", [b.dolgozoId]);
    if (!dolgozo[0]) throw ApiError.notFound("Szolgáltatást nyújtó dolgozó nem található, vagy inaktív.");

    const [tag] = await pool.query("SELECT Id FROM tagok WHERE Id=? AND Aktiv=1", [b.tagId]);
    if (!tag[0]) throw ApiError.notFound("Tag nem található, vagy inaktív.");

    const [result] = await pool.query(
      `INSERT INTO egyenifoglalkozasok
        (SzolgaltatasTipusId, DolgozoId, TagId, Idopont, Ar, Allapot, RogzitveAltalId, Megjegyzes)
       VALUES (?, ?, ?, ?, ?, ?, ?, ?)`,
      [
        b.szolgaltatasTipusId,
        b.dolgozoId,
        b.tagId,
        new Date(b.idopont),
        b.arFelulir ?? tipus.AlapertelmezettAr,
        EGYENI_ALLAPOT.toDb("Lefoglalva"),
        req.user.sub,
        b.megjegyzes ?? null,
      ]
    );

    const [rows] = await pool.query(`${FOGLALKOZAS_SELECT} WHERE f.Id=?`, [result.insertId]);
    res.json(toResponse(rows[0]));
  } catch (err) {
    next(err);
  }
}

export async function statuszFrissites(req, res, next) {
  try {
    const [rows] = await pool.query(`${FOGLALKOZAS_SELECT} WHERE f.Id=?`, [req.params.id]);
    const foglalkozas = rows[0];
    if (!foglalkozas) throw ApiError.notFound("Egyéni foglalkozás nem található.");

    const ujAllapot = EGYENI_ALLAPOT.toDb(req.body.allapot);
    if (foglalkozas.Allapot !== EGYENI_ALLAPOT.toDb("Lefoglalva")) {
      throw ApiError.conflict("Csak lefoglalt állapotú foglalkozás állapota módosítható.");
    }

    await pool.query("UPDATE egyenifoglalkozasok SET Allapot=? WHERE Id=?", [ujAllapot, req.params.id]);

    const [ujRows] = await pool.query(`${FOGLALKOZAS_SELECT} WHERE f.Id=?`, [req.params.id]);
    res.json(toResponse(ujRows[0]));
  } catch (err) {
    next(err);
  }
}

export async function getTagSajatjai(req, res, next) {
  try {
    const [rows] = await pool.query(`${FOGLALKOZAS_SELECT} WHERE f.TagId=? ORDER BY f.Idopont DESC`, [
      req.user.tagId,
    ]);
    res.json(rows.map(toResponse));
  } catch (err) {
    next(err);
  }
}

export async function getDolgozoSajatjai(req, res, next) {
  try {
    const [rows] = await pool.query(`${FOGLALKOZAS_SELECT} WHERE f.DolgozoId=? ORDER BY f.Idopont DESC`, [
      req.user.dolgozoId,
    ]);
    res.json(rows.map(toResponse));
  } catch (err) {
    next(err);
  }
}
