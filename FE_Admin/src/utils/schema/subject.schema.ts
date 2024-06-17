/* eslint-disable @typescript-eslint/no-explicit-any */

import * as yup from "yup";

const defaultError = "Trường này là bắt buộc";

export const subjectSchema = yup.object({
	grade: yup.string().trim().required(defaultError),
	subjectName: yup.string().trim().required(defaultError),
	subjectDescription: yup.string().trim(),
});
export type ISubjectSchema = yup.InferType<typeof subjectSchema>;
