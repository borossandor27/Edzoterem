import { Router } from "express";
import { authenticate } from "../middleware/auth.js";
import { login, me, jelszoValtoztatas } from "../controllers/authController.js";

const router = Router();

router.post("/login", login);
router.get("/me", authenticate, me);
router.post("/jelszo-valtoztatas", authenticate, jelszoValtoztatas);

export default router;
