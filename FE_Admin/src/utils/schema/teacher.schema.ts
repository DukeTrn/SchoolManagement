/* eslint-disable @typescript-eslint/no-explicit-any */

import * as yup from "yup";

const defaultError = "Trường này là bắt buộc";

export const teacherSchema = yup.object({
	teacherId: yup.string().trim().required(defaultError),
	fullName: yup.string().trim().required(defaultError),
	dob: yup.string().trim().required(defaultError),
	gender: yup.string().trim().required(defaultError),
	identificationNumber: yup.string().trim(),
	avatar: yup.string().trim(),
	status: yup.string().trim(),
	email: yup
		.string()
		.trim()
		.required(defaultError)
		.matches(
			/^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@[a-z0-9](?:[a-z0-9-]*[a-z0-9])?(\.[a-z0-9](?:[a-z0-9-]*[a-z0-9]))?$/,
			"Email không đúng định dạng"
		),
	level: yup.string().trim().required(defaultError),
	phoneNumber: yup.string().trim().required(defaultError),
	address: yup.string().trim().required(defaultError),
	ethnic: yup.string().trim().required(defaultError),
	timeStart: yup.string().trim().required(defaultError),
	timeEnd: yup.string().trim(),
});

export type ITeacherSchema = yup.InferType<typeof teacherSchema>;
