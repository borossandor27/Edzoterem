import { useAuth } from "../../context/AuthContext";

export function EdzoDashboardPage() {
  const { nev } = useAuth();
  return (
    <div>
      <h1>Üdv, {nev}!</h1>
      <p>Itt hirdetheted meg a csoportos foglalkozásaidat, és kezelheted a jelenlétet.</p>
    </div>
  );
}
