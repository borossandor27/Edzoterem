import type { ReactNode } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import { LoginPage } from "./routes/LoginPage";
import { TagDashboardPage } from "./routes/tag/TagDashboardPage";
import { TagAdataimPage } from "./routes/tag/TagAdataimPage";
import { TagBerletemPage } from "./routes/tag/TagBerletemPage";
import { TagFoglalasaimPage } from "./routes/tag/TagFoglalasaimPage";
import { EdzoDashboardPage } from "./routes/edzo/EdzoDashboardPage";
import { EdzoFoglalkozasaimPage } from "./routes/edzo/EdzoFoglalkozasaimPage";
import { EdzoCsoportosMeghirdetesPage } from "./routes/edzo/EdzoCsoportosMeghirdetesPage";
import { ProtectedRoute } from "./components/ProtectedRoute";
import { NavBar } from "./components/NavBar";
import { useAuth } from "./context/AuthContext";

function LayoutTaggal({ children }: { children: ReactNode }) {
  return (
    <>
      <NavBar />
      <main style={{ padding: 24 }}>{children}</main>
    </>
  );
}

export default function App() {
  const { bejelentkezve, szerepkor } = useAuth();

  return (
    <Routes>
      <Route path="/login" element={<LoginPage />} />

      <Route
        path="/tag/*"
        element={
          <ProtectedRoute megengedettSzerepkorok={["Tag"]}>
            <LayoutTaggal>
              <Routes>
                <Route index element={<TagDashboardPage />} />
                <Route path="adataim" element={<TagAdataimPage />} />
                <Route path="berletem" element={<TagBerletemPage />} />
                <Route path="foglalasaim" element={<TagFoglalasaimPage />} />
              </Routes>
            </LayoutTaggal>
          </ProtectedRoute>
        }
      />

      <Route
        path="/edzo/*"
        element={
          <ProtectedRoute megengedettSzerepkorok={["Edzo"]}>
            <LayoutTaggal>
              <Routes>
                <Route index element={<EdzoDashboardPage />} />
                <Route path="foglalkozasaim" element={<EdzoFoglalkozasaimPage />} />
                <Route path="meghirdetes" element={<EdzoCsoportosMeghirdetesPage />} />
              </Routes>
            </LayoutTaggal>
          </ProtectedRoute>
        }
      />

      <Route
        path="/"
        element={
          bejelentkezve ? (
            <Navigate to={szerepkor === "Tag" ? "/tag" : "/edzo"} replace />
          ) : (
            <Navigate to="/login" replace />
          )
        }
      />
    </Routes>
  );
}
