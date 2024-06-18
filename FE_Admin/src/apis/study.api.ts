import { ISuccessGetResponseApi } from "@/types/utils.type";
import http from "@/utils/http";

interface IBodyClassroom {
	searchValue: string;
	pageSize: number;
	pageNumber: number;
}

export const getStudyInfo = (academicYear: string, grade: number) =>
	http.post<
		ISuccessGetResponseApi<
			{
				classId: string;
				className: string;
				homeroomTeacherName: string;
			}[]
		>
	>(`assessment/${encodeURIComponent(academicYear)}/${grade}/classes`);

export const getStudyClassDetail = (
	body: IBodyClassroom,
	grade: number,
	semesterId: string,
	classId: string
) =>
	http.post<ISuccessGetResponseApi<any>>(
		`assessment/${grade}/${semesterId}/${classId}/students`,
		body
	);

export const getStudyStudentDetail = (
	grade: number,
	semesterId: string,
	classDetailId: string
) =>
	http.post<ISuccessGetResponseApi<any>>(
		`assessment/${grade}/${semesterId}/${classDetailId}/scores`
	);
