import jwt from "jsonwebtoken";
import { jwtConfig } from "../config/jwt.js";
import { ApiError } from "../utils/apiError.js";

export function authenticate(req, res, next) {
  const header = req.headers.authorization;
  if (!header?.startsWith("Bearer ")) {
    return next(ApiError.unauthorized("Hiányzó vagy érvénytelen Authorization fejléc."));
  }

  const token = header.slice("Bearer ".length);

  try {
    req.user = jwt.verify(token, jwtConfig.secret);
    next();
  } catch {
    next(ApiError.unauthorized("Érvénytelen vagy lejárt token."));
  }
}

export function requireRole(...szerepkorok) {
  return (req, res, next) => {
    if (!req.user || !szerepkorok.includes(req.user.szerepkor)) {
      return next(ApiError.forbidden("Nincs jogosultságod ehhez a művelethez."));
    }
    next();
  };
}
