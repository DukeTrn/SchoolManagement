import { IStudy, IClass } from "@/types/study.type";
import { IClassroom, IStudentClassroom } from "@/types/classroom.type";

import {
	ISuccessGetResponseApi,
	ISuccessResponseApi,
} from "@/types/utils.type";
import http from "@/utils/http";

interface IBodyClassroom {
	searchValue: string;
	pageSize: number;
	pageNumber: number;
}
type IClassDetailBody = IBodyClassroom & { status: number[] };

export const getStudentByAcademicYear = (body: IBodyClassroom, id: number) =>
	http.post<ISuccessResponseApi<IClassroom[]>>(`class/all/${id}`, body);

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
