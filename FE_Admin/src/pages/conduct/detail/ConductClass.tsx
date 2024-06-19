import { useEffect, useState } from "react";
import { getConductClass } from "@/apis/conduct.api";
import { useLocation, useNavigate } from "react-router-dom";
import Pagination from "@/components/pagination";
import { ColumnDef } from "@tanstack/react-table";
import { IClass } from "@/types/study.type";
import { TableDetails } from "@/components/table/Table";
import { Button } from "@/components/ui/button";
import { ArrowLeft } from "lucide-react";

const ConductClass = () => {
	const location = useLocation();
	const state = location?.state;
	const navigation = useNavigate();
	const [classInfo, setClassInfo] = useState<{
		classDetailId: string;
		studentId: string;
		studentName: string;
		conductId: string;
		conductName: string;
		feedback: string;
	}>();
	const [loading, setLoading] = useState<boolean>(false);
	const [pageSize, setPageSize] = useState<number>(10);
	const [pageNumber, setPageNumber] = useState<number>(1);
	const [totalPage] = useState<number>(1);

	const columns: ColumnDef<IClass>[] = [
		{
			accessorKey: "studentName",
			header: () => {
				return <div>Họ tên</div>;
			},
			cell: ({ row }) => <div>{row.getValue("studentName")}</div>,
			minSize: 200,
		},
		{
			accessorKey: "conductName",
			header: "Hạnh kiểm",
			cell: ({ row }) => <div>{row.getValue("conductName")}</div>,
		},
		{
			accessorKey: "feedback",
			header: "Nhận xét",
			cell: ({ row }) => <div>{row.getValue("feedback")}</div>,
		},
	];

	const initValue = {
		searchValue: "",
		pageSize: pageSize,
		pageNumber: pageNumber,
	};
	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize]);

	const handleGetData = () => {
		setLoading(true);
		getConductClass(
			initValue,
			Number(state.selectedField),
			state.semester,
			state.classId
		).then((res) => {
			setClassInfo(res.data?.data.dataList);
			setLoading(false);
		});
	};
	return (
		<>
			<div className="mb-5 flex justify-start gap-5">
				<Button onClick={() => navigation(-1)}>
					<ArrowLeft />
				</Button>
				<div className="mb-4 text-2xl font-medium">{`QUẢN LÝ HẠNH KIỂM LỚP ${state.className}`}</div>
			</div>
			<div>
				<TableDetails
					pageSize={pageSize}
					data={classInfo}
					columns={columns}
					loading={loading}
				/>
				<Pagination
					pageSize={pageSize}
					onChangePage={(value) => setPageNumber(Number(value))}
					onChangeRow={(value) => setPageSize(Number(value))}
					totalPageCount={totalPage}
				/>
			</div>
		</>
	);
};

export default ConductClass;
