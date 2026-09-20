import { Link } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export function NavBar() {
  const { nev, szerepkor, kijelentkezes } = useAuth();

  const linkek =
    szerepkor === "Tag"
      ? [
          { to: "/tag", label: "Kezdőlap" },
          { to: "/tag/adataim", label: "Adataim" },
          { to: "/tag/berletem", label: "Bérletem" },
          { to: "/tag/foglalasaim", label: "Foglalásaim" },
        ]
      : [
          { to: "/edzo", label: "Kezdőlap" },
          { to: "/edzo/foglalkozasaim", label: "Foglalkozásaim" },
          { to: "/edzo/meghirdetes", label: "Foglalkozás meghirdetése" },
        ];

  return (
    <nav style={{ display: "flex", gap: 16, padding: 16, borderBottom: "1px solid #D3D1C7", alignItems: "center" }}>
      <strong>Edzőterem</strong>
      {linkek.map((l) => (
        <Link key={l.to} to={l.to}>
          {l.label}
        </Link>
      ))}
      <span style={{ marginLeft: "auto" }}>
        {nev} ({szerepkor})
      </span>
      <button onClick={kijelentkezes}>Kijelentkezés</button>
    </nav>
  );
}
