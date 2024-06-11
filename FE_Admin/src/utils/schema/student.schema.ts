/* eslint-disable @typescript-eslint/no-explicit-any */

import * as yup from "yup";

const defaultError = "Trường này là bắt buộc";

export const studentSchema = yup.object({
	// email: yup
	// 	.string()
	// 	.required(defaultError)
	// 	.min(5, "Độ dài từ 5-160 kí tự")
	// 	.max(160, "Độ dài từ 5-160 kí tự")
	// 	.matches(
	// 		/^[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@[a-z0-9](?:[a-z0-9-]*[a-z0-9])?(\.[a-z0-9](?:[a-z0-9-]*[a-z0-9]))?$/,
	// 		"Email không đúng định dạng"
	// 	),
	studentId: yup.string().trim(),
	email: yup.string().trim(),
	fullName: yup.string().trim().required(defaultError),
	dob: yup.string().trim().required(defaultError),
	gender: yup.string().trim().required(defaultError),
	status: yup.string().trim().required(defaultError),
	identificationNumber: yup.string().trim(),
	avatar: yup.string().trim(),
	address: yup.string().trim().required(defaultError),
	ethnic: yup.string().trim().required(defaultError),
	phoneNumber: yup.string().trim().required(defaultError),
	academicYear: yup.string().trim().required(defaultError),
	fatherName: yup.string().trim().required(defaultError),
	fatherJob: yup.string().trim().required(defaultError),
	fatherEmail: yup.string().trim(),
	motherEmail: yup.string().trim(),
	fatherPhoneNumber: yup.string().trim().required(defaultError),
	motherName: yup.string().trim().required(defaultError),
	motherJob: yup.string().trim().required(defaultError),
	motherPhoneNumber: yup.string().trim().required(defaultError),
});

export type IStudentSchema = yup.InferType<typeof studentSchema>;
