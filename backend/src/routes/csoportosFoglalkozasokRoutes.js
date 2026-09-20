import { Router } from "express";
import { authenticate, requireRole } from "../middleware/auth.js";
import {
  getAll,
  getById,
  meghirdetes,
  jovahagyas,
  elutasitas,
  jelentkezes,
  jelentkezesTorlese,
  jelenlet,
  lezaras,
} from "../controllers/csoportosFoglalkozasokController.js";

const router = Router();

router.use(authenticate);

router.get("/", requireRole("Tulajdonos", "Recepcios"), getAll);
router.get("/:id", requireRole("Tulajdonos", "Recepcios", "Edzo"), getById);
router.post("/", requireRole("Edzo"), meghirdetes);
router.put("/:id/jovahagyas", requireRole("Tulajdonos"), jovahagyas);
router.put("/:id/elutasitas", requireRole("Tulajdonos"), elutasitas);
router.post("/:id/jelentkezes", requireRole("Tulajdonos", "Recepcios"), jelentkezes);
router.delete("/:id/jelentkezes/:tagId", requireRole("Tulajdonos", "Recepcios"), jelentkezesTorlese);
router.put("/:id/jelenlet", requireRole("Edzo"), jelenlet);
router.put("/:id/lezaras", requireRole("Edzo"), lezaras);

export default router;
