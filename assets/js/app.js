Vue.createApp({
	data() {
		return {
			mssv: '',
			maLop: '',
			listData: null,
			errMesg: null,
			showLoading: null
		}
	},
	methods: {
		search() {
			if (!this.mssv && !this.maLop) {
				this.errMesg = "Phải nhập mã sinh viên hoặc mã lớp";
				return;
			} else {
				this.errMesg = null;
			}
			this.showLoading = true;
			axios.get('/api/search', {
				params: {
					MaSinhVien: this.mssv,
					LopHoc: this.maLop
				}
			})
			.then(json => {
				this.listData = json.data;
				this.showLoading = false;
			})
			.catch(err => {
				this.showLoading = false;
			});
		}
	}
})
.mount('.container');