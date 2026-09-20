import { useAuth } from "../../context/AuthContext";

export function TagDashboardPage() {
  const { nev } = useAuth();
  return (
    <div>
      <h1>Üdv, {nev}!</h1>
      <p>Itt tekintheted meg a saját adataidat, a bérletedet és a foglalásaidat.</p>
    </div>
  );
}
