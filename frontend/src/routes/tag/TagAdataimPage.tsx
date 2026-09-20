import { useEffect, useState } from "react";
import { tagokApi } from "../../api/tagok";
import type { TagResponse } from "../../types";
import { ApiHiba } from "../../api/client";

export function TagAdataimPage() {
  const [adatok, setAdatok] = useState<TagResponse | null>(null);
  const [hiba, setHiba] = useState("");

  useEffect(() => {
    tagokApi
      .sajatAdataim()
      .then(setAdatok)
      .catch((err) => setHiba(err instanceof ApiHiba ? err.message : "Hiba az adatok betöltésekor."));
  }, []);

  if (hiba) return <p style={{ color: "red" }}>{hiba}</p>;
  if (!adatok) return <p>Betöltés...</p>;

  return (
    <div>
      <h1>Adataim</h1>
      <dl>
        <dt>Név</dt>
        <dd>{adatok.vezeteknev} {adatok.keresztnev}</dd>
        <dt>Születési dátum</dt>
        <dd>{adatok.szuletesiDatum}</dd>
        <dt>Cím</dt>
        <dd>{adatok.cim ?? "-"}</dd>
        <dt>Telefon</dt>
        <dd>{adatok.telefon ?? "-"}</dd>
        <dt>E-mail</dt>
        <dd>{adatok.email ?? "-"}</dd>
        <dt>Regisztráció dátuma</dt>
        <dd>{adatok.regisztracioDatuma}</dd>
        <dt>Vészhelyzeti kapcsolattartó</dt>
        <dd>{adatok.vhKapcsolattartoNev ?? "-"} ({adatok.vhKapcsolattartoTelefon ?? "-"})</dd>
      </dl>
      <p style={{ color: "gray" }}>
        Adatmódosítási igény esetén keresd a recepciót vagy az edződet – a doksi szerint a tag adatait a
        tulajdonos vagy az edző módosíthatja.
      </p>
    </div>
  );
}
