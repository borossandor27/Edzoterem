import bcrypt from "bcryptjs";
import { pool } from "../db/pool.js";
import { ApiError } from "../utils/apiError.js";
import { NEM, SZEREPKOR } from "../utils/enums.js";
import { jelszoGeneralas } from "../utils/jelszoGeneralo.js";

function toResponse(row) {
  return {
    id: row.Id,
    vezeteknev: row.Vezeteknev,
    keresztnev: row.Keresztnev,
    szuletesiDatum: row.SzuletesiDatum,
    cim: row.Cim,
    telefon: row.Telefon,
    email: row.Email,
    regisztracioDatuma: row.RegisztracioDatuma,
    vhKapcsolattartoNev: row.VhKapcsolattartoNev,
    vhKapcsolattartoTelefon: row.VhKapcsolattartoTelefon,
    nem: NEM.fromDb(row.Nem),
    megjegyzes: row.Megjegyzes,
    aktiv: !!row.Aktiv,
  };
}

export async function getAll(req, res, next) {
  try {
    const [rows] = await pool.query("SELECT * FROM tagok ORDER BY Vezeteknev, Keresztnev");
    res.json(rows.map(toResponse));
  } catch (err) {
    next(err);
  }
}

export async function getById(req, res, next) {
  try {
    const [rows] = await pool.query("SELECT * FROM tagok WHERE Id=?", [req.params.id]);
    if (!rows[0]) throw ApiError.notFound("Tag nem található.");
    res.json(toResponse(rows[0]));
  } catch (err) {
    next(err);
  }
}

export async function getSajatAdataim(req, res, next) {
  try {
    const [rows] = await pool.query("SELECT * FROM tagok WHERE Id=?", [req.user.tagId]);
    if (!rows[0]) throw ApiError.notFound("Tag nem található.");
    res.json(toResponse(rows[0]));
  } catch (err) {
    next(err);
  }
}

async function generaljEgyediFelhasznalonevet(vezeteknev, keresztnev) {
  const alap = `${vezeteknev}.${keresztnev}`.toLowerCase().replace(/\s+/g, "");
  let jelolt = alap;
  let i = 1;
  // egyediség ellenőrzése, ütközésnél számmal növelve (pl. kiss.marton, kiss.marton2, ...)
  while (true) {
    const [rows] = await pool.query("SELECT Id FROM felhasznalok WHERE Felhasznalonev=?", [jelolt]);
    if (!rows[0]) return jelolt;
    i += 1;
    jelolt = `${alap}${i}`;
  }
}

export async function regisztracio(req, res, next) {
  const connection = await pool.getConnection();
  try {
    await connection.beginTransaction();
    const b = req.body;

    const [result] = await connection.query(
      `INSERT INTO tagok
        (Vezeteknev, Keresztnev, SzuletesiDatum, Cim, Telefon, Email, RegisztracioDatuma,
         VhKapcsolattartoNev, VhKapcsolattartoTelefon, Nem, Megjegyzes, Aktiv)
       VALUES (?, ?, ?, ?, ?, ?, CURDATE(), ?, ?, ?, ?, 1)`,
      [
        b.vezeteknev,
        b.keresztnev,
        b.szuletesiDatum,
        b.cim ?? null,
        b.telefon ?? null,
        b.email ?? null,
        b.vhKapcsolattartoNev ?? null,
        b.vhKapcsolattartoTelefon ?? null,
        NEM.toDb(b.nem),
        b.megjegyzes ?? null,
      ]
    );
    const tagId = result.insertId;

    const felhasznalonev = await generaljEgyediFelhasznalonevet(b.vezeteknev, b.keresztnev);
    const ideiglenesJelszo = jelszoGeneralas();
    const jelszoHash = await bcrypt.hash(ideiglenesJelszo, 11);

    await connection.query(
      "INSERT INTO felhasznalok (Felhasznalonev, JelszoHash, Szerepkor, TagId, Aktiv, JelszoIdeiglenes) VALUES (?, ?, ?, ?, 1, 1)",
      [felhasznalonev, jelszoHash, SZEREPKOR.toDb("Tag"), tagId]
    );

    await connection.commit();

    const [tagRows] = await pool.query("SELECT * FROM tagok WHERE Id=?", [tagId]);

    res.status(201).json({
      tag: toResponse(tagRows[0]),
      felhasznalonev,
      ideiglenesJelszo,
    });
  } catch (err) {
    await connection.rollback();
    next(err);
  } finally {
    connection.release();
  }
}

export async function update(req, res, next) {
  try {
    const [existing] = await pool.query("SELECT Id FROM tagok WHERE Id=?", [req.params.id]);
    if (!existing[0]) throw ApiError.notFound("Tag nem található.");

    const b = req.body;
    await pool.query(
      `UPDATE tagok SET Vezeteknev=?, Keresztnev=?, SzuletesiDatum=?, Cim=?, Telefon=?, Email=?,
        VhKapcsolattartoNev=?, VhKapcsolattartoTelefon=?, Nem=?, Megjegyzes=? WHERE Id=?`,
      [
        b.vezeteknev,
        b.keresztnev,
        b.szuletesiDatum,
        b.cim ?? null,
        b.telefon ?? null,
        b.email ?? null,
        b.vhKapcsolattartoNev ?? null,
        b.vhKapcsolattartoTelefon ?? null,
        NEM.toDb(b.nem),
        b.megjegyzes ?? null,
        req.params.id,
      ]
    );

    const [rows] = await pool.query("SELECT * FROM tagok WHERE Id=?", [req.params.id]);
    res.json(toResponse(rows[0]));
  } catch (err) {
    next(err);
  }
}

export async function inaktivalas(req, res, next) {
  try {
    const [result] = await pool.query("UPDATE tagok SET Aktiv=0 WHERE Id=?", [req.params.id]);
    if (result.affectedRows === 0) throw ApiError.notFound("Tag nem található.");
    res.status(204).send();
  } catch (err) {
    next(err);
  }
}
