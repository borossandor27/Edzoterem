import { Router } from "express";
import { authenticate, requireRole } from "../middleware/auth.js";
import {
  getSzolgaltatasTipusok,
  createSzolgaltatasTipus,
  rogzites,
  statuszFrissites,
} from "../controllers/egyeniFoglalkozasokController.js";

const router = Router();

// Lásd a berletekRoutes.js megjegyzését: nincs router.use(authenticate) blanket middleware,
// mert ez a router "/" alatt van felcsatlakoztatva a routes/index.js-ben.
router.get("/szolgaltatastipusok", authenticate, getSzolgaltatasTipusok);
router.post("/szolgaltatastipusok", authenticate, requireRole("Tulajdonos"), createSzolgaltatasTipus);

router.post("/egyeni-foglalkozasok", authenticate, requireRole("Tulajdonos", "Recepcios"), rogzites);
router.put("/egyeni-foglalkozasok/:id/statusz", authenticate, requireRole("Tulajdonos", "Recepcios"), statuszFrissites);

export default router;
