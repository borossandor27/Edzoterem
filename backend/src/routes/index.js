import { Router } from "express";
import authRoutes from "./authRoutes.js";
import dolgozokRoutes from "./dolgozokRoutes.js";
import edzokRoutes from "./edzokRoutes.js";
import tagokRoutes from "./tagokRoutes.js";
import berletekRoutes from "./berletekRoutes.js";
import csoportosFoglalkozasokRoutes from "./csoportosFoglalkozasokRoutes.js";
import egyeniFoglalkozasokRoutes from "./egyeniFoglalkozasokRoutes.js";

const router = Router();

router.use("/auth", authRoutes);
router.use("/dolgozok", dolgozokRoutes);
router.use("/edzok", edzokRoutes);
router.use("/tagok", tagokRoutes);
router.use("/csoportos-foglalkozasok", csoportosFoglalkozasokRoutes);
router.use("/", berletekRoutes);
router.use("/", egyeniFoglalkozasokRoutes);

export default router;
