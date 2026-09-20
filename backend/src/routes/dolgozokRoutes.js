import { Router } from "express";
import { authenticate, requireRole } from "../middleware/auth.js";
import {
  getAll,
  getById,
  create,
  update,
  kileptetes,
  getMunkarend,
  addMunkarend,
} from "../controllers/dolgozokController.js";

const router = Router();

router.use(authenticate, requireRole("Tulajdonos"));

router.get("/", getAll);
router.get("/:id", getById);
router.post("/", create);
router.put("/:id", update);
router.delete("/:id", kileptetes);
router.get("/:id/munkarend", getMunkarend);
router.post("/:id/munkarend", addMunkarend);

export default router;
