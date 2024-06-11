import { Button } from "@/components/ui/button";
import { useTranslation } from "react-i18next";

export default function Home() {
	const { t, i18n } = useTranslation();
	const currrentLanguage = i18n.language;
	return (
		<div>
			<div className="text-3xl font-semibold uppercase">
				Chào mừng bạn đến với trang quản lý đào tạo
			</div>
		</div>
	);
}
