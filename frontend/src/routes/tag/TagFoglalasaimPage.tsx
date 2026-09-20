import { useEffect, useState } from "react";
import { tagokApi } from "../../api/tagok";
import type { CsoportosFoglalkozasResponse, EgyeniFoglalkozasResponse } from "../../types";
import { ApiHiba } from "../../api/client";

export function TagFoglalasaimPage() {
  const [csoportos, setCsoportos] = useState<CsoportosFoglalkozasResponse[]>([]);
  const [egyeni, setEgyeni] = useState<EgyeniFoglalkozasResponse[]>([]);
  const [hiba, setHiba] = useState("");

  useEffect(() => {
    Promise.all([tagokApi.sajatCsoportosFoglalkozasok(), tagokApi.sajatEgyeniFoglalkozasok()])
      .then(([c, e]) => {
        setCsoportos(c);
        setEgyeni(e);
      })
      .catch((err) => setHiba(err instanceof ApiHiba ? err.message : "Hiba a foglalások betöltésekor."));
  }, []);

  return (
    <div>
      <h1>Foglalásaim</h1>
      {hiba && <p style={{ color: "red" }}>{hiba}</p>}

      <h2>Csoportos foglalkozások</h2>
      <table>
        <thead>
          <tr><th>Megnevezés</th><th>Edző</th><th>Időpont</th><th>Helyszín</th><th>Állapot</th></tr>
        </thead>
        <tbody>
          {csoportos.map((f) => (
            <tr key={f.id}>
              <td>{f.megnevezes}</td><td>{f.edzoNev}</td><td>{f.idopont}</td><td>{f.helyszin}</td><td>{f.statusz}</td>
            </tr>
          ))}
        </tbody>
      </table>

      <h2>Egyéni foglalkozások</h2>
      <table>
        <thead>
          <tr><th>Szolgáltatás</th><th>Szolgáltató</th><th>Időpont</th><th>Ár</th><th>Állapot</th></tr>
        </thead>
        <tbody>
          {egyeni.map((f) => (
            <tr key={f.id}>
              <td>{f.szolgaltatasNev}</td><td>{f.dolgozoNev}</td><td>{f.idopont}</td><td>{f.ar} Ft</td><td>{f.allapot}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
