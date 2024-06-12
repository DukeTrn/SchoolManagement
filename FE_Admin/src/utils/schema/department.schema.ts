/* eslint-disable @typescript-eslint/no-explicit-any */

import * as yup from "yup";

const defaultError = "Trường này là bắt buộc";

export const departmentSchema = yup.object({
	departmentId: yup.string().trim().required(defaultError),
	subjectName: yup.string().trim().required(defaultError),
	description: yup.string().trim(),
	notification: yup.string().trim(),
});

export type IDepartmentSchema = yup.InferType<typeof departmentSchema>;
