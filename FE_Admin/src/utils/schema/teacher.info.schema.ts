/* eslint-disable @typescript-eslint/no-explicit-any */

import * as yup from "yup";

export const teacherInfoSchema = yup.object({
	score: yup.number(),
	weight: yup.number(),
	feedback: yup.string().trim(),
	semesterId: yup.string().trim(),
	subjectId: yup.number(),
	classDetailId: yup.string().trim(),
});

export type ITeacherInfoSchema = yup.InferType<typeof teacherInfoSchema>;
