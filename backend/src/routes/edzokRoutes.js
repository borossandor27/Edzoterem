import { Router } from "express";
import { authenticate, requireRole } from "../middleware/auth.js";
import { getEdzok, createEdzo, updateEdzo, getElerhetoseg, addElerhetoseg } from "../controllers/dolgozokController.js";
import { meEdzoSajatjai } from "../controllers/csoportosFoglalkozasokController.js";
import { getDolgozoSajatjai } from "../controllers/egyeniFoglalkozasokController.js";

const router = Router();

router.use(authenticate);

router.get("/me/csoportos-foglalkozasok", requireRole("Edzo"), meEdzoSajatjai);
router.get("/me/egyeni-foglalkozasok", requireRole("Edzo"), getDolgozoSajatjai);

router.get("/", requireRole("Tulajdonos", "Recepcios"), getEdzok);
router.post("/:dolgozoId", requireRole("Tulajdonos"), createEdzo);
router.put("/:edzoId", requireRole("Tulajdonos"), updateEdzo);
router.get("/:edzoId/elerhetoseg", requireRole("Tulajdonos", "Recepcios", "Edzo"), getElerhetoseg);
router.put("/:edzoId/elerhetoseg", requireRole("Tulajdonos", "Edzo"), addElerhetoseg);

export default router;
