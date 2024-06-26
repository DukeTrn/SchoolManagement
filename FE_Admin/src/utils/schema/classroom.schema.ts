/* eslint-disable @typescript-eslint/no-explicit-any */

import * as yup from "yup";

const defaultError = "Trường này là bắt buộc";

export const classroomSchema = yup.object({
	grade: yup.string().trim().required(defaultError),
	className: yup.string().trim().required(defaultError),
	academicYear: yup.string().trim().required(defaultError),
	homeroomTeacherId: yup.string().trim(),
});

export type IClassroomSchema = yup.InferType<typeof classroomSchema>;

export const classroomStudentDetailSchema = yup.object({
	studentsValue: yup.array().length(1, defaultError).required(defaultError),
});

export type IClassroomStudentDetailSchema = yup.InferType<
	typeof classroomStudentDetailSchema
>;

export const classroomFreeSchema = yup.object({
	classIds: yup.string().trim().required(defaultError),
});

export type IClassroomFreeSchema = yup.InferType<typeof classroomFreeSchema>;
