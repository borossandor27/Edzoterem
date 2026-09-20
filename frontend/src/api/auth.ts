import { apiClient } from "./client";
import type { LoginResponse } from "../types";

export function bejelentkezes(felhasznalonev: string, jelszo: string) {
  return apiClient.post<LoginResponse>("/api/auth/login", { felhasznalonev, jelszo });
}

export function jelszoValtoztatas(regiJelszo: string, ujJelszo: string) {
  return apiClient.post<void>("/api/auth/jelszo-valtoztatas", { regiJelszo, ujJelszo });
}
