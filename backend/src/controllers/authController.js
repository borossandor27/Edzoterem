import bcrypt from "bcryptjs";
import jwt from "jsonwebtoken";
import { pool } from "../db/pool.js";
import { jwtConfig } from "../config/jwt.js";
import { ApiError } from "../utils/apiError.js";
import { SZEREPKOR } from "../utils/enums.js";

async function felhasznaloNevvel(felhasznaloId) {
  const [rows] = await pool.query(
    `SELECT f.Id, f.Felhasznalonev, f.Szerepkor, f.DolgozoId, f.TagId, f.JelszoIdeiglenes, f.JelszoHash,
            d.Vezeteknev AS DolgozoVezeteknev, d.Keresztnev AS DolgozoKeresztnev,
            t.Vezeteknev AS TagVezeteknev, t.Keresztnev AS TagKeresztnev
     FROM felhasznalok f
     LEFT JOIN dolgozok d ON d.Id = f.DolgozoId
     LEFT JOIN tagok t ON t.Id = f.TagId
     WHERE f.Id = ?`,
    [felhasznaloId]
  );
  return rows[0] ?? null;
}

function nevbol(row) {
  if (row.DolgozoVezeteknev) return `${row.DolgozoVezeteknev} ${row.DolgozoKeresztnev}`;
  if (row.TagVezeteknev) return `${row.TagVezeteknev} ${row.TagKeresztnev}`;
  return row.Felhasznalonev;
}

export async function login(req, res, next) {
  try {
    const { felhasznalonev, jelszo } = req.body;

    const [rows] = await pool.query(
      `SELECT f.Id, f.Felhasznalonev, f.JelszoHash, f.Szerepkor, f.Aktiv, f.JelszoIdeiglenes,
              f.DolgozoId, f.TagId,
              d.Vezeteknev AS DolgozoVezeteknev, d.Keresztnev AS DolgozoKeresztnev,
              t.Vezeteknev AS TagVezeteknev, t.Keresztnev AS TagKeresztnev
       FROM felhasznalok f
       LEFT JOIN dolgozok d ON d.Id = f.DolgozoId
       LEFT JOIN tagok t ON t.Id = f.TagId
       WHERE f.Felhasznalonev = ?`,
      [felhasznalonev]
    );
    const felhasznalo = rows[0];

    if (!felhasznalo || !felhasznalo.Aktiv || !(await bcrypt.compare(jelszo ?? "", felhasznalo.JelszoHash))) {
      throw ApiError.unauthorized("Hibás felhasználónév vagy jelszó.");
    }

    await pool.query("UPDATE felhasznalok SET UtolsoBejelentkezes = NOW() WHERE Id = ?", [felhasznalo.Id]);

    const { DolgozoId, TagId } = felhasznalo;

    const nev = nevbol(felhasznalo);
    const szerepkorNev = SZEREPKOR.fromDb(felhasznalo.Szerepkor);

    const token = jwt.sign(
      {
        sub: felhasznalo.Id,
        szerepkor: szerepkorNev,
        nev,
        felhasznalonev: felhasznalo.Felhasznalonev,
        dolgozoId: DolgozoId ?? undefined,
        tagId: TagId ?? undefined,
      },
      jwtConfig.secret,
      { expiresIn: jwtConfig.expiresIn }
    );

    res.json({
      token,
      szerepkor: szerepkorNev,
      nev,
      jelszoIdeiglenes: !!felhasznalo.JelszoIdeiglenes,
    });
  } catch (err) {
    next(err);
  }
}

export async function me(req, res, next) {
  try {
    const row = await felhasznaloNevvel(req.user.sub);
    if (!row) throw ApiError.notFound("Felhasználó nem található.");

    res.json({
      id: row.Id,
      felhasznalonev: row.Felhasznalonev,
      szerepkor: SZEREPKOR.fromDb(row.Szerepkor),
      nev: nevbol(row),
    });
  } catch (err) {
    next(err);
  }
}

export async function jelszoValtoztatas(req, res, next) {
  try {
    const { regiJelszo, ujJelszo } = req.body;
    const row = await felhasznaloNevvel(req.user.sub);
    if (!row) throw ApiError.notFound("Felhasználó nem található.");

    if (!(await bcrypt.compare(regiJelszo ?? "", row.JelszoHash))) {
      throw ApiError.badRequest("A régi jelszó nem megfelelő.");
    }

    const ujHash = await bcrypt.hash(ujJelszo, 11);
    await pool.query("UPDATE felhasznalok SET JelszoHash = ?, JelszoIdeiglenes = 0 WHERE Id = ?", [
      ujHash,
      row.Id,
    ]);

    res.status(204).send();
  } catch (err) {
    next(err);
  }
}
