import { apiClient } from "./client";
import type { CsoportosFoglalkozasResponse, EgyeniFoglalkozasResponse } from "../types";

export const edzokApi = {
  sajatCsoportosFoglalkozasok: () =>
    apiClient.get<CsoportosFoglalkozasResponse[]>("/api/edzok/me/csoportos-foglalkozasok"),
  sajatEgyeniFoglalkozasok: () =>
    apiClient.get<EgyeniFoglalkozasResponse[]>("/api/edzok/me/egyeni-foglalkozasok"),
};
