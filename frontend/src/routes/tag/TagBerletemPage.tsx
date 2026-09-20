import { useEffect, useState } from "react";
import { tagokApi } from "../../api/tagok";
import type { BerletResponse } from "../../types";
import { ApiHiba } from "../../api/client";

export function TagBerletemPage() {
  const [berletek, setBerletek] = useState<BerletResponse[]>([]);
  const [hiba, setHiba] = useState("");

  useEffect(() => {
    tagokApi
      .sajatBerlet()
      .then(setBerletek)
      .catch((err) => setHiba(err instanceof ApiHiba ? err.message : "Hiba a bérletek betöltésekor."));
  }, []);

  return (
    <div>
      <h1>Bérletem</h1>
      {hiba && <p style={{ color: "red" }}>{hiba}</p>}
      <table>
        <thead>
          <tr>
            <th>Típus</th>
            <th>Kezdő dátum</th>
            <th>Lejárat</th>
            <th>Hátralévő alkalmak</th>
            <th>Állapot</th>
          </tr>
        </thead>
        <tbody>
          {berletek.map((b) => (
            <tr key={b.id} style={{ color: b.lejaro ? "red" : undefined }}>
              <td>{b.berletTipusNev}</td>
              <td>{b.kezdoDatum}</td>
              <td>{b.lejaratiDatum}</td>
              <td>{b.hatralevoAlkalmak ?? "-"}</td>
              <td>{b.statusz}{b.lejaro ? " (hamarosan lejár)" : ""}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
