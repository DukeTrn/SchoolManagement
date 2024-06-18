import { ISuccessGetResponseApi } from "@/types/utils.type";
import http from "@/utils/http";

interface IBodyClassroom {
	searchValue: string;
	pageSize: number;
	pageNumber: number;
}

export const getConductInfo = (grade: number, semester: string) =>
	http.post<ISuccessGetResponseApi<[]>>(
		`conduct/${grade}/${semester}/classes`
	);

export const getConductClass = (
	body: IBodyClassroom,
	grade: number,
	semesterId: string,
	classId: string
) =>
	http.post<ISuccessGetResponseApi<any>>(
		`conduct/${grade}/${semesterId}/${classId}/students`,
		body
	);
