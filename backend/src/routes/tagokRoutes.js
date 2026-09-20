import { Router } from "express";
import { authenticate, requireRole } from "../middleware/auth.js";
import { getAll, getById, getSajatAdataim, regisztracio, update, inaktivalas } from "../controllers/tagokController.js";
import { getTagBerletei, getSajatBerlet } from "../controllers/berletekController.js";
import { meTagSajatjai } from "../controllers/csoportosFoglalkozasokController.js";
import { getTagSajatjai } from "../controllers/egyeniFoglalkozasokController.js";

const router = Router();

router.use(authenticate);

// "me" végpontok előbb, hogy ne az "/:id" mintázat fogja el a "me" szót
router.get("/me", requireRole("Tag"), getSajatAdataim);
router.get("/me/berlet", requireRole("Tag"), getSajatBerlet);
router.get("/me/csoportos-foglalkozasok", requireRole("Tag"), meTagSajatjai);
router.get("/me/egyeni-foglalkozasok", requireRole("Tag"), getTagSajatjai);

router.get("/", requireRole("Tulajdonos", "Recepcios"), getAll);
router.post("/", requireRole("Tulajdonos", "Recepcios"), regisztracio);
router.get("/:id", requireRole("Tulajdonos", "Recepcios", "Edzo"), getById);
router.put("/:id", requireRole("Tulajdonos", "Recepcios", "Edzo"), update);
router.put("/:id/inaktivalas", requireRole("Tulajdonos"), inaktivalas);
router.get("/:tagId/berletek", requireRole("Tulajdonos", "Recepcios", "Edzo"), getTagBerletei);

export default router;
