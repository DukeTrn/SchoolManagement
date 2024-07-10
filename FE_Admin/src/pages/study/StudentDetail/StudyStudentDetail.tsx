import { useEffect, useState } from "react";
import { getStudyStudentDetail } from "@/apis/study.api";
import { useLocation, useNavigate } from "react-router-dom";
import { ColumnDef } from "@tanstack/react-table";
import { IClass } from "@/types/study.type";
import { TableDetails } from "@/components/table/Table";
import { mean } from "lodash";
import { Button } from "@/components/ui/button";
import { ArrowLeft } from "lucide-react";

const StudyStudentDetail = () => {
	const location = useLocation();
	const navigation = useNavigate();
	const state = location?.state;
	const [loading, setLoading] = useState<boolean>(false);
	const [pageSize] = useState<number>(10);
	const [pageNumber] = useState<number>(1);
	const [student, setStudent] = useState<any>([]);
	const [point, setPoint] = useState<any>();
	const columns: ColumnDef<IClass>[] = [
		{
			accessorKey: "subjectName",
			header: () => {
				return <div>Môn học</div>;
			},
			cell: ({ row }) => <div>{row.getValue("subjectName")}</div>,
			minSize: 200,
		},
		{
			accessorKey: "weight1",
			header: () => {
				return <div>ĐĐGtx</div>;
			},
			cell: ({ row }) => {
				const weights = row.getValue("weight1") as any[];
				return (
					<div>
						{weights.map((item, index) => (
							<div key={index} className="inline-block">
								<div>{`${item.score} \u00A0`}</div>
							</div>
						))}
					</div>
				);
			},
			minSize: 200,
		},
		{
			accessorKey: "weight2",
			header: () => {
				return <div>ĐĐGgk</div>;
			},
			cell: ({ row }) => {
				const weights = row.getValue("weight2") as any[];
				return (
					<div>
						{weights.map((item, index) => (
							<div key={index} className="inline-block">
								<div>{`${item.score} \u00A0`}</div>
							</div>
						))}
					</div>
				);
			},
		},
		{
			accessorKey: "weight3",
			header: () => {
				return <div>ĐĐGck</div>;
			},
			cell: ({ row }) => {
				const weights = row.getValue("weight3") as any[];
				return (
					<div>
						{weights.map((item, index) => (
							<div key={index} className="inline-block">
								<div>{`${item.score} \u00A0`}</div>
							</div>
						))}
					</div>
				);
			},
		},
		{
			accessorKey: "average",
			header: () => {
				return <div>ĐTBmhk</div>;
			},
			cell: ({ row }) => <div>{row.getValue("average")}</div>,
		},
	];

	const formatNumberWithCommas = (
		inputNumber: number | null | undefined,
		decimalPlaces: number = 2
	) => {
		if (inputNumber === undefined || inputNumber === null) {
			return "";
		}
		const isNegative = inputNumber < 0;
		const absoluteNumber = Math.abs(inputNumber);

		let formattedDecimalNumber = 0;

		const multiplier = Math.pow(10, decimalPlaces);
		formattedDecimalNumber =
			Math.round(absoluteNumber * multiplier) / multiplier;

		let formattedNumber = formattedDecimalNumber.toFixed(decimalPlaces);

		const parts = formattedNumber.split(".");
		let integerPart = parts[0];
		let decimalPart = parts[1] || "";

		let formattedInteger = "";
		let count = 0;

		for (let i = integerPart.length - 1; i >= 0; i--) {
			formattedInteger = integerPart[i] + formattedInteger;
			count++;
			if (count % 3 === 0 && i !== 0) {
				formattedInteger = "," + formattedInteger;
			}
		}

		if (decimalPart.length > 0) {
			formattedInteger += "." + decimalPart;
		}

		return isNegative && formattedDecimalNumber !== 0
			? "-" + formattedInteger
			: formattedInteger;
	};

	const academicPerformance = (value: number) => {
		switch (true) {
			case value >= 8:
				return "Tốt";
			case value < 8 && value >= 6.5:
				return "Khá";
			case value < 6.5 && value >= 3.5:
				return "Đạt";
			default:
				return "Chưa Đạt";
		}
	};

	useEffect(() => {
		handleGetData();
	}, [pageNumber, pageSize]);
	const handleGetData = () => {
		setLoading(true);
		getStudyStudentDetail(
			state.grade,
			state.semester,
			state.classDetailId
		).then((res) => {
			setStudent(res.data?.data);
			setLoading(false);
		});
	};
	useEffect(() => {
		const b = student.map((item: any) => item.average) as number[];
		setPoint(b);
	}, [student]);

	return (
		<>
			<div className="mb-5 flex justify-start gap-5">
				<Button onClick={() => navigation(-1)}>
					<ArrowLeft />
				</Button>
				<div className="mb-4 text-2xl font-medium">{`KẾT QUẢ HỌC TẬP HỌC SINH ${state.fullName.toUpperCase()} `}</div>
			</div>
			<div className=" [&>[role=checkbox]]:translate-y-[2px]text-sm mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0">
				{`Học lực: ${academicPerformance(
					Number(formatNumberWithCommas(mean(point)))
				)}`}
			</div>
			<div className="mb-5 mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
				{`Điểm TB: ${formatNumberWithCommas(mean(point))}`}
			</div>
			<div>
				<TableDetails
					pageSize={pageSize}
					data={student}
					columns={columns}
					//onChange={handleChange}
					loading={loading}
				/>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					Chú Thích
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• ĐĐGtx: Điểm Đánh Giá thường xuyên
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• ĐĐGgk: Điểm Đánh Giá giữa kỳ
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• ĐĐGck: Điểm Đánh Giá cuối kỳ
				</div>
				<div className="mt-2 h-5 px-2 text-left align-middle text-sm text-sm font-medium text-muted-foreground [&:has([role=checkbox])]:pr-0 [&>[role=checkbox]]:translate-y-[2px]">
					• ĐTBmhk: Điểm Trung Bình môn học kỳ
				</div>
			</div>
		</>
	);
};

export default StudyStudentDetail;
