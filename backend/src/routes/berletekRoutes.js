import { Router } from "express";
import { authenticate, requireRole } from "../middleware/auth.js";
import { getTipusok, createTipus, updateTipus, hozzarendeles, getLejarok } from "../controllers/berletekController.js";

const router = Router();

// Fontos: NEM router.use(authenticate) (blanket) — ez a router "/" alatt van felcsatlakoztatva
// a routes/index.js-ben, így minden nem egyező útvonalra is lefutna, mielőtt még
// eldőlne, hogy egyáltalán ide tartozik-e a kérés (lásd: 401 helyett 404 az illesztetlen
// útvonalakon). Ezért minden route-on külön hívjuk meg.
router.get("/berlettipusok", authenticate, getTipusok);
router.post("/berlettipusok", authenticate, requireRole("Tulajdonos"), createTipus);
router.put("/berlettipusok/:id", authenticate, requireRole("Tulajdonos"), updateTipus);

router.post("/berletek", authenticate, requireRole("Tulajdonos", "Recepcios"), hozzarendeles);
router.get("/berletek/lejaro", authenticate, getLejarok);

export default router;
