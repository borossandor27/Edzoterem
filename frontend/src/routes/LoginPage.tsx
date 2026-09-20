import { useState, type FormEvent } from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import { jelszoValtoztatas } from "../api/auth";
import { ApiHiba } from "../api/client";

export function LoginPage() {
  const { bejelentkezve, jelszoIdeiglenes, szerepkor, bejelentkezes, jelszoCsereMegtortent } = useAuth();

  const [felhasznalonev, setFelhasznalonev] = useState("");
  const [jelszo, setJelszo] = useState("");
  const [ujJelszo, setUjJelszo] = useState("");
  const [hiba, setHiba] = useState("");
  const [folyamatban, setFolyamatban] = useState(false);

  if (bejelentkezve && !jelszoIdeiglenes) {
    return <Navigate to={szerepkor === "Tag" ? "/tag" : "/edzo"} replace />;
  }

  async function bejelentkezesKezelo(e: FormEvent) {
    e.preventDefault();
    setHiba("");
    if (!felhasznalonev.trim() || !jelszo) {
      setHiba("Add meg a felhasználónevet és a jelszót.");
      return;
    }

    setFolyamatban(true);
    try {
      await bejelentkezes(felhasznalonev, jelszo);
    } catch (err) {
      setHiba(err instanceof ApiHiba ? err.message : "Nem sikerült kapcsolódni a szerverhez.");
    } finally {
      setFolyamatban(false);
    }
  }

  async function jelszoCsereKezelo(e: FormEvent) {
    e.preventDefault();
    setHiba("");
    if (ujJelszo.length < 8) {
      setHiba("Az új jelszónak legalább 8 karakter hosszúnak kell lennie.");
      return;
    }

    setFolyamatban(true);
    try {
      await jelszoValtoztatas(jelszo, ujJelszo);
      jelszoCsereMegtortent();
    } catch (err) {
      setHiba(err instanceof ApiHiba ? err.message : "Nem sikerült megváltoztatni a jelszót.");
    } finally {
      setFolyamatban(false);
    }
  }

  if (bejelentkezve && jelszoIdeiglenes) {
    return (
      <div style={{ maxWidth: 360, margin: "80px auto" }}>
        <h2>Kötelező jelszócsere</h2>
        <p>Ez az első bejelentkezésed ideiglenes jelszóval történt. Adj meg egy új jelszót a folytatáshoz.</p>
        <form onSubmit={jelszoCsereKezelo}>
          <div>
            <label>Új jelszó</label>
            <input type="password" value={ujJelszo} onChange={(e) => setUjJelszo(e.target.value)} />
          </div>
          {hiba && <p style={{ color: "red" }}>{hiba}</p>}
          <button type="submit" disabled={folyamatban}>
            Jelszó mentése
          </button>
        </form>
      </div>
    );
  }

  return (
    <div style={{ maxWidth: 360, margin: "80px auto" }}>
      <h2>Edzőterem – bejelentkezés</h2>
      <p>Edzők és tagok számára.</p>
      <form onSubmit={bejelentkezesKezelo}>
        <div>
          <label>Felhasználónév</label>
          <input value={felhasznalonev} onChange={(e) => setFelhasznalonev(e.target.value)} />
        </div>
        <div>
          <label>Jelszó</label>
          <input type="password" value={jelszo} onChange={(e) => setJelszo(e.target.value)} />
        </div>
        {hiba && <p style={{ color: "red" }}>{hiba}</p>}
        <button type="submit" disabled={folyamatban}>
          Bejelentkezés
        </button>
      </form>
    </div>
  );
}
