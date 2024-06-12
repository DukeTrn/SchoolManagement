import axios, { AxiosError } from "axios";
import HttpStatusCode from "@/constants/httpStatusCode.enum";

export const isAxiosError = <T>(error: unknown): error is AxiosError<T> => {
	// eslint-disable-next-line import/no-named-as-default-member
	return axios.isAxiosError(error);
};

//FormError la kieu tra ve cua data
export const isAxiosUnprocessableEntityError = <FormError>(
	error: unknown
): error is AxiosError<FormError> => {
	return (
		isAxiosError(error) &&
		error.response?.status === HttpStatusCode.UnprocessableEntity
	);
};

export const convertDateISO = (dateStr: string) => {
	const [day, month, year] = dateStr.split("/").map(Number);
	const date = new Date(Date.UTC(year, month - 1, day));
	return date.toISOString();
};

export const downloadFile = (res: string, name: string) => {
	const url = window.URL.createObjectURL(new Blob([res]));
	const link = document.createElement("a");
	link.href = url;
	link.setAttribute("download", `${name}_${Date.now()}.xlsx`);
	document.body.appendChild(link);
	link.click();
};
