import { useState, type FormEvent } from "react";
import { csoportosFoglalkozasokApi } from "../../api/csoportosFoglalkozasok";
import { ApiHiba } from "../../api/client";

export function EdzoCsoportosMeghirdetesPage() {
  const [megnevezes, setMegnevezes] = useState("");
  const [idopont, setIdopont] = useState("");
  const [helyszin, setHelyszin] = useState("");
  const [maxLetszam, setMaxLetszam] = useState(10);
  const [uzenet, setUzenet] = useState("");
  const [hiba, setHiba] = useState("");

  async function kuldes(e: FormEvent) {
    e.preventDefault();
    setHiba("");
    setUzenet("");

    if (!megnevezes.trim() || !idopont || !helyszin.trim() || maxLetszam < 1) {
      setHiba("Tölts ki minden mezőt.");
      return;
    }

    try {
      await csoportosFoglalkozasokApi.meghirdetes({
        megnevezes,
        idopont: new Date(idopont).toISOString(),
        helyszin,
        maxLetszam,
      });
      setUzenet("A foglalkozás meghirdetve, jóváhagyásra vár a tulajdonosnál.");
      setMegnevezes("");
      setIdopont("");
      setHelyszin("");
      setMaxLetszam(10);
    } catch (err) {
      setHiba(err instanceof ApiHiba ? err.message : "Nem sikerült meghirdetni a foglalkozást.");
    }
  }

  return (
    <div>
      <h1>Csoportos foglalkozás meghirdetése</h1>
      <form onSubmit={kuldes} style={{ display: "flex", flexDirection: "column", gap: 8, maxWidth: 360 }}>
        <label>
          Megnevezés
          <input value={megnevezes} onChange={(e) => setMegnevezes(e.target.value)} />
        </label>
        <label>
          Időpont
          <input type="datetime-local" value={idopont} onChange={(e) => setIdopont(e.target.value)} />
        </label>
        <label>
          Helyszín
          <input value={helyszin} onChange={(e) => setHelyszin(e.target.value)} />
        </label>
        <label>
          Maximális létszám
          <input type="number" min={1} value={maxLetszam} onChange={(e) => setMaxLetszam(Number(e.target.value))} />
        </label>
        {hiba && <p style={{ color: "red" }}>{hiba}</p>}
        {uzenet && <p style={{ color: "green" }}>{uzenet}</p>}
        <button type="submit">Meghirdetés</button>
      </form>
    </div>
  );
}
