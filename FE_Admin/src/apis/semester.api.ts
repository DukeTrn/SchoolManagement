import {
	IFilterSemesters,
	ISemester,
	ISemesterDetail,
} from "@/types/semester.type";
import {
	ISuccessGetResponseApi,
	ISuccessResponseApi,
} from "@/types/utils.type";
import http from "@/utils/http";

interface ISemesterDetailBody {
	searchValue: string;
	pageSize: number;
	pageNumber: number;
	grades: number[];
}
export const getAllSemesters = (body: {
	pageSize: number;
	pageNumber: number;
}) => http.post<ISuccessResponseApi<ISemester[]>>("semester/all", body);

export const createSemester = (body: ISemester) =>
	http.post("semester/create", body);

export const updateSemester = (body: ISemester) =>
	http.put(`semester/${body?.semesterId}`, body);

export const deleteSemester = (id: string) => http.delete(`semester/${id}`);

export const getSemesterDetails = (body: ISemesterDetailBody, id: string) =>
	http.post<ISuccessResponseApi<Omit<ISemesterDetail, "academicYear">[]>>(
		`semesterdetail/all?semesterId=${id}`,
		body
	);

export const createSemesterDetail = (body: {
	semesterId: string;
	classId: string[];
}) => http.post("semesterdetail/create", body);

export const removeSemesterDetail = (body: string[]) =>
	http.delete("semesterdetail/remove", {
		data: body,
	});
``;

export const getClassSemester = (id: string) =>
	http.get<{ data: { classId: string; className: string }[] }>(
		`semesterdetail/filter/${id}`
	);

export const getAllFilterSemester = () =>
	http.get<ISuccessGetResponseApi<IFilterSemesters[]>>(
		"semester/filter/allsemesters"
	);

export const getSemesterInAcademicYear = (academicYear: string) =>
	http.get<ISuccessGetResponseApi<IFilterSemesters[]>>(
		`semester/filter/semesters?academicYear=${encodeURIComponent(
			academicYear
		)}`
	);
