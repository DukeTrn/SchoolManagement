/* eslint-disable @typescript-eslint/no-explicit-any */

import * as yup from "yup";

const defaultError = "Trường này là bắt buộc";

export const teacherInfoSchema = yup.object({
	score: yup.number(),
	weight: yup.number(),
	feedback: yup.string().trim(),
	semesterId: yup.string().trim(),
	subjectId: yup.number(),
	classDetailId: yup.string().trim(),
});

export type ITeacherInfoSchema = yup.InferType<typeof teacherInfoSchema>;

export const conductSchema = yup.object({
	semester: yup.string().trim().required(defaultError),
	conduct: yup.string().required(defaultError),
	comment: yup.string().trim(),
});

export type IConductSchema = yup.InferType<typeof conductSchema>;
