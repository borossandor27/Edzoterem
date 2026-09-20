import type { ReactNode } from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "../context/AuthContext";
import type { Szerepkor } from "../types";

interface Props {
  megengedettSzerepkorok: Szerepkor[];
  children: ReactNode;
}

export function ProtectedRoute({ megengedettSzerepkorok, children }: Props) {
  const { bejelentkezve, szerepkor } = useAuth();

  if (!bejelentkezve) {
    return <Navigate to="/login" replace />;
  }

  if (szerepkor && !megengedettSzerepkorok.includes(szerepkor)) {
    return <Navigate to="/login" replace />;
  }

  return <>{children}</>;
}
