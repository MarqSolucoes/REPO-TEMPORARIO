<template>
  <div class="main">
    <div class="dropzone-container">
      <input type="file" multiple name="file" id="fileInput" class="hidden-input" @change="onChange" ref="file"
        accept=".pdf,.jpg,.jpeg,.png" />

      <label for="fileInput" class="file-label">
        <div>Clique para escolher seus arquivos</div>
      </label>

      <div class="preview-container mt-4" v-if="files.length">
        <div v-for="file in files" :key="file.name" class="preview-card">
          <div>
            <p :title="file.name">
              {{ file.name }}
            </p>
          </div>
          <div>
            <button class="ml-2" type="button" @click="remove(files.indexOf(file))" title="Cancelar envio">
              <b>&times;</b>
            </button>
            <button class="ml-2" type="button" @click="uploadFiles()" title="Enviar envio">
              <b>OK</b>
            </button>
          </div>
          <br />
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import ApiService from "@/services/api.service.js";
export default {
  data() {
    return {
      files: [],
    };
  },
  methods: {
    onChange() {
      this.files = [...this.$refs.file.files];
    },

    remove(i) {
      this.files.splice(i, 1);
    },

    uploadFiles() {
      const files = this.files;
      const formData = new FormData();
      files.forEach((file) => {
        formData.append("arquivo", file);
      });

      ApiService.uploadFile(formData, (result) => {
        if (result.status != 200) {
          this.$swal("", result.message, "error");
        } else {
          this.files = [];
          this.$swal("Arquivo enviado com sucesso", result.message, "success");
        }
      });
    },
  },
};
</script>

<style>
.main {
  display: flex;
  flex-grow: 1;
  align-items: center;

  justify-content: center;
  text-align: center;
}

.dropzone-container {
  padding: 4rem;

  border: 1px solid #e2e8f0;
}

.hidden-input {
  opacity: 0;
  overflow: hidden;
  position: absolute;
  width: 1px;
  height: 1px;
}

.file-label {
  font-size: 20px;
  display: block;
  cursor: pointer;
}

.preview-container {
  display: flex;
  margin-top: 2rem;
}

.preview-card {
  display: flex;
  padding: 5px;
  margin-left: 5px;
}
</style>