/* eslint-disable @typescript-eslint/no-explicit-any */

import * as yup from "yup";

const defaultError = "Trường này là bắt buộc";

export const semesterSchema = yup.object({
	semesterId: yup.string().trim().required(defaultError),
	semesterName: yup.string().trim().required(defaultError),
	timeStart: yup.string().trim().required(defaultError),
	timeEnd: yup.string().trim(),
});

export type ISemesterSchema = yup.InferType<typeof semesterSchema>;

export const semesterDetailSchema = yup.object({
	class: yup.array().length(1, defaultError).required(defaultError),
});

export type ISemesterDetailSchema = yup.InferType<typeof semesterDetailSchema>;
