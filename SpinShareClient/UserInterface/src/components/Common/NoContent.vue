<template>
    <section class="no-content">
        <span class="mdi mdi-close-circle-outline"></span>
        <p>{{ text }}</p>
    </section>
</template>

<script setup>
defineProps({
    text: String,
});
</script>

<style lang="scss" scoped>
.no-content {
    display: flex;
    flex-direction: column;
    gap: 15px;
    justify-content: center;
    align-items: center;
    padding: 50px 0;

    & .mdi {
        font-size: 48px;
    }
    & p {
        color: rgba(var(--colorBaseText), 0.6);
        line-height: 1.25rem;
    }
}
</style>
