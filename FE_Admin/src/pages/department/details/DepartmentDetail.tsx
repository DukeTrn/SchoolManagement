import { MultiSelect } from "@/components/multiselect/MultiSelect";
import { TableDetails } from "@/components/table/Table";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { ITeacher } from "@/types/teacher.type";
import { ColumnDef } from "@tanstack/react-table";
import { Search } from "lucide-react";
import React, { useState } from "react";

const statusList = [
	{ value: "0", label: "Đang dạy" },
	{ value: "1", label: "Nghỉ  việc" },
];

type ITeacherTable = Pick<
	ITeacher,
	| "teacherId"
	| "fullName"
	| "dob"
	| "phoneNumber"
	| "email"
	| "level"
	| "status"
>;

const data: ITeacherTable[] = [
	{
		teacherId: "10012024",
		fullName: "",
		dob: "",
		phoneNumber: "",
		email: "",
		level: "",
		status: "",
	},
];

const DepartmentDetail = () => {
	const [searchValue, setSearchValue] = useState("");
	const [selectedField, setSelectedField] = useState<string[]>([]);

	return (
		<>
			<div className="mb-4 text-2xl font-medium">QUẢN LÝ BỘ MÔN TOÁN</div>
			<div className="mb-5 flex justify-between">
				<div className="relative min-w-[295px]">
					<Search className="absolute left-2 top-2.5 h-4 w-4 " />
					<Input
						width={300}
						placeholder="Tìm kiếm"
						className="pl-8"
						onChange={(e) => setSearchValue(e.target?.value)}
					/>
				</div>
				<MultiSelect
					options={statusList}
					onValueChange={setSelectedField}
					placeholder="Tình trạng học tập"
					variant="inverted"
					animation={2}
					maxCount={0}
					width={230}
				/>
			</div>
			<div className="mb-5 flex justify-between">
				<div>
					<Button>Xóa</Button>
				</div>
			</div>
			<div>
				<TableDetails
					data={data}
					columns={columns}
					onChange={handleChange}
					loading={false}
				/>
			</div>
		</>
	);
};

export default DepartmentDetail;
