export type ISemester = {
	semesterId: string;
	semesterName: string;
	timeStart: string;
	timeEnd?: string;
	academicYear?: string;
};

export type ISemesterDetail = {
	id?: string;
	classId: string;
	className: string;
	homeroomTeacherId: string;
	homeroomTeacherName: string;
	grade: string;
	academicYear: string;
};

export type IFilterSemesters = {
	semesterId: string;
	semesterName: string;
};
