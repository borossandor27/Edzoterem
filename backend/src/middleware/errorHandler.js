import { ApiError } from "../utils/apiError.js";

export function errorHandler(err, req, res, next) {
  if (err instanceof ApiError) {
    return res.status(err.status).json({ uzenet: err.message });
  }

  console.error(err);
  res.status(500).json({ uzenet: "Váratlan szerverhiba történt." });
}

export function notFoundHandler(req, res) {
  res.status(404).json({ uzenet: "Az útvonal nem található." });
}
