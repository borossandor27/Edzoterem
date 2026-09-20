import { apiClient } from "./client";
import type { CsoportosFoglalkozasResponse } from "../types";

export interface MeghirdetesRequest {
  megnevezes: string;
  idopont: string;
  helyszin: string;
  maxLetszam: number;
}

export const csoportosFoglalkozasokApi = {
  meghirdetes: (request: MeghirdetesRequest) =>
    apiClient.post<CsoportosFoglalkozasResponse>("/api/csoportos-foglalkozasok", request),
  jelenletRogzites: (id: number, jelenletek: { tagId: number; jelenVolt: boolean }[]) =>
    apiClient.put<CsoportosFoglalkozasResponse>(`/api/csoportos-foglalkozasok/${id}/jelenlet`, { jelenletek }),
  lezaras: (id: number) => apiClient.put<CsoportosFoglalkozasResponse>(`/api/csoportos-foglalkozasok/${id}/lezaras`),
};
