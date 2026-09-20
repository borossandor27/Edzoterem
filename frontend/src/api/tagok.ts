import { apiClient } from "./client";
import type { BerletResponse, CsoportosFoglalkozasResponse, EgyeniFoglalkozasResponse, TagResponse } from "../types";

export const tagokApi = {
  sajatAdataim: () => apiClient.get<TagResponse>("/api/tagok/me"),
  sajatBerlet: () => apiClient.get<BerletResponse[]>("/api/tagok/me/berlet"),
  sajatCsoportosFoglalkozasok: () =>
    apiClient.get<CsoportosFoglalkozasResponse[]>("/api/tagok/me/csoportos-foglalkozasok"),
  sajatEgyeniFoglalkozasok: () =>
    apiClient.get<EgyeniFoglalkozasResponse[]>("/api/tagok/me/egyeni-foglalkozasok"),
};
