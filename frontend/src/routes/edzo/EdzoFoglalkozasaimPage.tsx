import { useEffect, useState } from "react";
import { edzokApi } from "../../api/edzok";
import { csoportosFoglalkozasokApi } from "../../api/csoportosFoglalkozasok";
import type { CsoportosFoglalkozasResponse } from "../../types";
import { ApiHiba } from "../../api/client";

export function EdzoFoglalkozasaimPage() {
  const [foglalkozasok, setFoglalkozasok] = useState<CsoportosFoglalkozasResponse[]>([]);
  const [jelenlet, setJelenlet] = useState<Record<number, boolean>>({});
  const [hiba, setHiba] = useState("");

  async function betoltes() {
    try {
      const lista = await edzokApi.sajatCsoportosFoglalkozasok();
      setFoglalkozasok(lista);
    } catch (err) {
      setHiba(err instanceof ApiHiba ? err.message : "Hiba a foglalkozások betöltésekor.");
    }
  }

  useEffect(() => {
    betoltes();
  }, []);

  function jelenletValtas(tagId: number, ertek: boolean) {
    setJelenlet((prev) => ({ ...prev, [tagId]: ertek }));
  }

  async function jelenletMentese(foglalkozas: CsoportosFoglalkozasResponse) {
    try {
      const jelenletek = foglalkozas.jelentkezesek.map((j) => ({
        tagId: j.tagId,
        jelenVolt: jelenlet[j.tagId] ?? j.jelenVolt ?? false,
      }));
      await csoportosFoglalkozasokApi.jelenletRogzites(foglalkozas.id, jelenletek);
      await betoltes();
    } catch (err) {
      setHiba(err instanceof ApiHiba ? err.message : "Nem sikerült rögzíteni a jelenlétet.");
    }
  }

  async function lezaras(id: number) {
    try {
      await csoportosFoglalkozasokApi.lezaras(id);
      await betoltes();
    } catch (err) {
      setHiba(err instanceof ApiHiba ? err.message : "Nem sikerült lezárni a foglalkozást.");
    }
  }

  return (
    <div>
      <h1>Saját csoportos foglalkozásaim</h1>
      {hiba && <p style={{ color: "red" }}>{hiba}</p>}

      {foglalkozasok.map((f) => (
        <div key={f.id} style={{ border: "1px solid #D3D1C7", padding: 12, marginBottom: 12 }}>
          <strong>{f.megnevezes}</strong> – {f.idopont} – {f.helyszin} – állapot: {f.statusz}
          {f.statusz === "Elutasitva" && f.elutasitasIndoka && <p>Elutasítás indoka: {f.elutasitasIndoka}</p>}

          {f.statusz === "Jovahagyva" && (
            <div style={{ marginTop: 8 }}>
              <h4>Jelenlét</h4>
              {f.jelentkezesek.length === 0 && <p>Nincs jelentkező.</p>}
              {f.jelentkezesek.map((j) => (
                <label key={j.tagId} style={{ display: "block" }}>
                  <input
                    type="checkbox"
                    checked={jelenlet[j.tagId] ?? j.jelenVolt ?? false}
                    onChange={(e) => jelenletValtas(j.tagId, e.target.checked)}
                  />
                  {j.tagNev}
                </label>
              ))}
              <button onClick={() => jelenletMentese(f)} style={{ marginTop: 8 }}>
                Jelenlét mentése
              </button>
              <button onClick={() => lezaras(f.id)} style={{ marginTop: 8, marginLeft: 8 }}>
                Foglalkozás lezárása
              </button>
            </div>
          )}
        </div>
      ))}
    </div>
  );
}
