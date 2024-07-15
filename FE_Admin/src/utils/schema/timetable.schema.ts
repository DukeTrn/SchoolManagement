import * as yup from "yup";

const defaultError = "Trường này là bắt buộc";

export const timeTableSchema = yup.object({
	subject: yup.string().trim().required(defaultError),
	day: yup.string().trim().required(defaultError),
	period: yup.string().trim().required(defaultError),
});

export const editTimeTableSchema = yup.object({
	day: yup.string().trim().required(defaultError),
	period: yup.string().trim().required(defaultError),
});
export type ITimetableSchema = yup.InferType<typeof timeTableSchema>;

export type IEditTimetableSchema = yup.InferType<typeof editTimeTableSchema>;
