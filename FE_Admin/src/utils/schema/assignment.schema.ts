/* eslint-disable @typescript-eslint/no-explicit-any */

import * as yup from "yup";

const defaultError = "Trường này là bắt buộc";

export const assignmentSchema = yup.object({
	classroom: yup.string().trim().required(defaultError),
	teacher: yup.string().trim().required(defaultError),
});

export type IAssignmentSchema = yup.InferType<typeof assignmentSchema>;
