/* eslint-disable @typescript-eslint/no-explicit-any */

import * as yup from "yup";

export const studySchema = yup.object({
	className: yup.string().trim(),
	description: yup.string().trim(),
	homeroomTeacherId: yup.string().trim(),
	academicYear: yup.string().trim(),
});

export type IStudySchema = yup.InferType<typeof studySchema>;
